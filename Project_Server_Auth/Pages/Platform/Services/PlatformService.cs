using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Business;
using DAL.Models.GeneralModels;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.Platform.Dtos;
using pr_srv_names.Pages.Platform.Interfaces;
using pr_srv_names.Pages.Shared.Seo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace pr_srv_names.Pages.Platform.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<PlatformCreateDto> _createValidator;
        private readonly IValidator<PlatformUpdateDto> _updateValidator;
        private readonly IValidator<PlatformPageRequestDto> _pageRequestValidator;

        public PlatformService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<PlatformService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<PlatformCreateDto> createValidator,
            IValidator<PlatformUpdateDto> updateValidator,
            IValidator<PlatformPageRequestDto> pageRequestValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _pageRequestValidator = pageRequestValidator;
        }

        private string GetCorrelationId() =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        public async Task<PlatformPagedResponseDto> GetPagedAsync(PlatformPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new FluentValidation.ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.Platforms.GetQueryable();

            if (request.LanguageId.HasValue)
            {
                query = query.Where(x => x.Translations.Any(t => t.LanguageId == request.LanguageId.Value));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(x => 
                    x.Name.ToLower().Contains(search) || 
                    x.Code.ToLower().Contains(search) ||
                    x.Translations.Any(t => t.Name.ToLower().Contains(search))
                );
            }

            var total = await query.CountAsync();

            // Сортировка
            query = request.SortBy switch
            {
                PlatformSortField.Name => request.SortDirection == Models.SortDirection.Asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                PlatformSortField.Code => request.SortDirection == Models.SortDirection.Asc ? query.OrderBy(x => x.Code) : query.OrderByDescending(x => x.Code),
                PlatformSortField.SortOrder => request.SortDirection == Models.SortDirection.Asc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                _ => query.OrderBy(x => x.SortOrder)
            };

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.Translations.Where(t => request.LanguageId == null || t.LanguageId == request.LanguageId))
                    .ThenInclude(t => t.Language)
                .ToListAsync();

            var dtos = items.Select(item => {
                var dto = _mapper.Map<PlatformItemDto>(item);
                var trans = item.Translations.FirstOrDefault(t => t.LanguageId == request.LanguageId) 
                            ?? item.Translations.FirstOrDefault();
                dto.LocalizedName = trans?.Name;
                return dto;
            });

                return new PlatformPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка платформ. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<PlatformDetailDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Platforms.GetWithTranslationsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("Platform", $"Платформа с ID {id} не найдена.");

            return _mapper.Map<PlatformDetailDto>(entity);
        }

        public async Task<PlatformDetailDto> CreateAsync(PlatformCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new FluentValidation.ValidationException(validationResult.Errors);

            if (!await IsCodeUniqueAsync(dto.Code))
                throw new ConflictException("Платформа с таким системным кодом уже существует.", "Platform", "Code");

            try
            {
                var entity = _mapper.Map<DAL.Models.Business.Platform>(dto);
                SyncTranslations(entity, dto.Translations);

                await _unitOfWork.Platforms.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var created = await _unitOfWork.Platforms.GetWithTranslationsAndSeoAsync(entity.Id);
                return _mapper.Map<PlatformDetailDto>(created!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании платформы. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<PlatformDetailDto> UpdateAsync(PlatformUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new FluentValidation.ValidationException(validationResult.Errors);

            if (!await IsCodeUniqueAsync(dto.Code, dto.Id))
                throw new ConflictException("Платформа с таким системным кодом уже существует.", "Platform", "Code");

            var entity = await _unitOfWork.Platforms.GetWithTranslationsAndSeoAsync(dto.Id);
            if (entity == null) throw new NotFoundException("Platform", $"Платформа с ID {dto.Id} не найдена.");

            try
            {
                _mapper.Map(dto, entity);
                SyncTranslations(entity, dto.Translations);

                _unitOfWork.Platforms.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updated = await _unitOfWork.Platforms.GetWithTranslationsAndSeoAsync(entity.Id);
                return _mapper.Map<PlatformDetailDto>(updated!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении платформы ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Platforms.GetFirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new NotFoundException("Platform", $"Платформа с ID {id} не найдена.");

            _unitOfWork.Platforms.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null) =>
            await _unitOfWork.Platforms.IsCodeUniqueAsync(code, excludeId);

        private void SyncTranslations(DAL.Models.Business.Platform entity, List<PlatformTranslationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Translations.FirstOrDefault(t => t.LanguageId == dto.LanguageId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                    existing.UpdatedAt = DateTime.UtcNow;

                    if (dto.SeoData != null)
                    {
                        if (existing.SeoData == null)
                            existing.SeoData = _mapper.Map<SeoData>(dto.SeoData);
                        else
                            _mapper.Map(dto.SeoData, existing.SeoData);
                        
                        existing.SeoData.AutoFillRelatedFields();
                    }
                    else if (existing.SeoData != null)
                    {
                        _unitOfWork.GetRepository<SeoData>().Delete(existing.SeoData);
                        existing.SeoData = null;
                        existing.SeoDataId = null;
                    }
                }
                else
                {
                    var newTrans = _mapper.Map<PlatformTranslation>(dto);
                    if (dto.SeoData != null)
                    {
                        newTrans.SeoData = _mapper.Map<SeoData>(dto.SeoData);
                        newTrans.SeoData.AutoFillRelatedFields();
                    }
                    entity.Translations.Add(newTrans);
                }
            }

            var dtoLangIds = dtos.Select(d => d.LanguageId).ToList();
            var toRemove = entity.Translations.Where(t => !dtoLangIds.Contains(t.LanguageId)).ToList();
            foreach (var remove in toRemove)
            {
                entity.Translations.Remove(remove);
            }
        }
    }
}
