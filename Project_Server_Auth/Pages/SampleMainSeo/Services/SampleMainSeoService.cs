using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.SampleMainSeo.Dtos;
using pr_srv_names.Pages.SampleMainSeo.Interfaces;
using pr_srv_names.Pages.Shared.Seo.Dtos;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models;
using pr_srv_names.Pages.Shared.Seo.Extensions;
using DAL.Models.SampleModels;
using DAL.Models.GeneralModels;

namespace pr_srv_names.Pages.SampleMainSeo.Services
{
    public class SampleMainSeoService : ISampleMainSeoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SampleMainSeoService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<SampleMainSeoCreateDto> _createValidator;
        private readonly IValidator<SampleMainSeoUpdateDto> _updateValidator;
        private readonly IValidator<SampleMainSeoPageRequestDto> _pageRequestValidator;

        public SampleMainSeoService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SampleMainSeoService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<SampleMainSeoCreateDto> createValidator,
            IValidator<SampleMainSeoUpdateDto> updateValidator,
            IValidator<SampleMainSeoPageRequestDto> pageRequestValidator)
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

        public async Task<SampleMainSeoPagedResponseDto> GetPagedAsync(SampleMainSeoPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.GetRepository<DAL.Models.SampleModels.SampleMainSeo>().GetQueryable();

                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x => x.Descriptions.Any(d => d.LanguageAppId == request.LanguageId.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x => 
                        x.Name.ToLower().Contains(search) || 
                        (x.SystemCode != null && x.SystemCode.ToLower().Contains(search)) ||
                        x.Descriptions.Any(d => d.Name.ToLower().Contains(search))
                    );
                }

                var total = await query.CountAsync();
                
                // Простейшая сортировка для примера
                query = request.SortDirection == SortDirection.Asc 
                    ? query.OrderBy(x => x.Name) 
                    : query.OrderByDescending(x => x.Name);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Descriptions.Where(d => request.LanguageId == null || d.LanguageAppId == request.LanguageId))
                        .ThenInclude(d => d.SeoData)
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<SampleMainSeoItemDto>(item);
                    var desc = item.Descriptions.FirstOrDefault(d => d.LanguageAppId == request.LanguageId) 
                               ?? item.Descriptions.FirstOrDefault();
                    dto.LocalizedName = desc?.Name;
                    dto.SeoScore = desc?.SeoData?.CalculateOptimizationScore() ?? 0;
                    return dto;
                });

                return new SampleMainSeoPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка SampleMainSeo. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleMainSeoDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.SamplesMainSeo.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("SampleMainSeo", $"Запись с ID {id} не найдена.");

            return _mapper.Map<SampleMainSeoDetailDto>(entity);
        }

        public async Task<SampleMainSeoDetailDto> CreateAsync(SampleMainSeoCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name, dto.SystemCode);

            try
            {
                var entity = _mapper.Map<DAL.Models.SampleModels.SampleMainSeo>(dto);
                
                foreach (var desc in entity.Descriptions)
                {
                    var dtoDesc = dto.Descriptions.FirstOrDefault(d => d.LanguageAppId == desc.LanguageAppId);
                    if (dtoDesc?.SeoData != null)
                    {
                        desc.SeoData = _mapper.Map<SeoData>(dtoDesc.SeoData);
                        desc.SeoData.AutoFillRelatedFields(); // Демонстрация использования расширений
                    }
                }

                await _unitOfWork.SamplesMainSeo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _unitOfWork.SamplesMainSeo.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<SampleMainSeoDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании SampleMainSeo. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleMainSeoDetailDto> UpdateAsync(SampleMainSeoUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name, dto.SystemCode, dto.Id);

            var entity = await _unitOfWork.SamplesMainSeo.GetWithDescriptionsAndSeoAsync(dto.Id);
            if (entity == null) throw new NotFoundException("SampleMainSeo", $"Запись с ID {dto.Id} не найдена.");

            try
            {
                _mapper.Map(dto, entity);
                SyncDescriptionsWithSeo(entity, dto.Descriptions);

                _unitOfWork.SamplesMainSeo.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.SamplesMainSeo.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<SampleMainSeoDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении SampleMainSeo ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.SamplesMainSeo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException("SampleMainSeo", $"Запись с ID {id} не найдена.");

            _unitOfWork.SamplesMainSeo.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null) =>
            await _unitOfWork.SamplesMainSeo.IsNameUniqueAsync(name, excludeId);

        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null) =>
            await _unitOfWork.SamplesMainSeo.IsSystemCodeUniqueAsync(code, excludeId);

        private async Task CheckUniqueness(string name, string? systemCode, int? excludeId = null)
        {
            if (!await IsNameUniqueAsync(name, excludeId))
                throw new ConflictException("Запись с таким техническим именем уже существует.", "SampleMainSeo", "Name");

            if (!string.IsNullOrWhiteSpace(systemCode) && !await IsSystemCodeUniqueAsync(systemCode, excludeId))
                throw new ConflictException("Запись с таким системным кодом уже существует.", "SampleMainSeo", "SystemCode");
        }

        private void SyncDescriptionsWithSeo(DAL.Models.SampleModels.SampleMainSeo entity, List<SampleMainDescriptionSeoDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Descriptions.FirstOrDefault(d => d.LanguageAppId == dto.LanguageAppId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                    existing.UpdatedAt = DateTime.UtcNow;

                    if (dto.SeoData != null)
                    {
                        if (existing.SeoData == null)
                        {
                            existing.SeoData = _mapper.Map<SeoData>(dto.SeoData);
                        }
                        else
                        {
                            _mapper.Map(dto.SeoData, existing.SeoData);
                        }
                        existing.SeoData.AutoFillRelatedFields();
                    }
                    else if (existing.SeoData != null)
                    {
                        // Удаляем SEO, если оно пришло пустым (опционально, зависит от бизнес-логики)
                        // В данном случае лучше оставить или занулить.
                        // План говорит: "универсальный плагин".
                        _unitOfWork.GetRepository<SeoData>().Delete(existing.SeoData);
                        existing.SeoData = null;
                        existing.SeoDataId = null;
                    }
                }
                else
                {
                    var newDesc = _mapper.Map<SampleMainDescriptionSeo>(dto);
                    newDesc.CreatedAt = DateTime.UtcNow;
                    newDesc.UpdatedAt = DateTime.UtcNow;
                    
                    if (dto.SeoData != null)
                    {
                        newDesc.SeoData = _mapper.Map<SeoData>(dto.SeoData);
                        newDesc.SeoData.AutoFillRelatedFields();
                    }
                    
                    entity.Descriptions.Add(newDesc);
                }
            }

            var dtoLangIds = dtos.Select(d => d.LanguageAppId).ToList();
            var toRemove = entity.Descriptions.Where(d => !dtoLangIds.Contains(d.LanguageAppId)).ToList();
            foreach (var remove in toRemove)
            {
                entity.Descriptions.Remove(remove);
            }
        }
    }
}
