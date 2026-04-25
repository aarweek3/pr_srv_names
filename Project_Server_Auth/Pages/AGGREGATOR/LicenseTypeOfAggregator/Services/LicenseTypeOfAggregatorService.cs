using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using DAL.Models.GeneralModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Interfaces;
using pr_srv_names.Pages.Shared.Seo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Project_Server_Auth.Services.Maintenance.Interfaces;
using LicenseEntity = DAL.Models.Aggregator.LicenseTypeOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Services
{
    public class LicenseTypeOfAggregatorService : ILicenseTypeOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LicenseTypeOfAggregatorService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<LicenseTypeOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<LicenseTypeOfAggregatorUpdateDto> _updateValidator;
        private readonly IValidator<LicenseTypeOfAggregatorPageRequestDto> _pageRequestValidator;
        private readonly IMaintenanceSeeder _maintenanceSeeder;

        public LicenseTypeOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<LicenseTypeOfAggregatorService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<LicenseTypeOfAggregatorCreateDto> createValidator,
            IValidator<LicenseTypeOfAggregatorUpdateDto> updateValidator,
            IValidator<LicenseTypeOfAggregatorPageRequestDto> pageRequestValidator,
            IMaintenanceSeeder maintenanceSeeder)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _pageRequestValidator = pageRequestValidator;
            _maintenanceSeeder = maintenanceSeeder;
        }

        private string GetCorrelationId() =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        public async Task<LicenseTypeOfAggregatorPagedResponseDto> GetPagedAsync(LicenseTypeOfAggregatorPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.LicenseTypesOfAggregator.GetQueryable();

                if (request.ShowDeleted)
                {
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);
                }

                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x => x.Localizations.Any(l => l.LanguageOfAggregatorId == request.LanguageId.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x => 
                        x.CanonicalName.ToLower().Contains(search) || 
                        x.Slug.ToLower().Contains(search) ||
                        x.Localizations.Any(l => l.Name.ToLower().Contains(search))
                    );
                }

                var total = await query.CountAsync();
                query = ApplySorting(query, request);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                        .ThenInclude(l => l.LanguageOfAggregator)
                    .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                        .ThenInclude(l => l.SeoData)
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<LicenseTypeOfAggregatorItemDto>(item);
                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId) 
                              ?? item.Localizations.FirstOrDefault();
                    dto.LocalizedName = loc?.Name;
                    return dto;
                });

                return new LicenseTypeOfAggregatorPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка типов лицензий. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<LicenseTypeOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.LicenseTypesOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("LicenseTypeOfAggregator", $"Тип лицензии с ID {id} не найден.");

            return _mapper.Map<LicenseTypeOfAggregatorDetailDto>(entity);
        }

        public async Task<LicenseTypeOfAggregatorDetailDto> CreateAsync(LicenseTypeOfAggregatorCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.CanonicalName, dto.Slug);

            try
            {
                var entity = _mapper.Map<LicenseEntity>(dto);
                
                foreach (var loc in entity.Localizations)
                {
                    var dtoLoc = dto.Localizations.FirstOrDefault(d => d.LanguageOfAggregatorId == loc.LanguageOfAggregatorId);
                    if (dtoLoc?.SeoData != null)
                    {
                        loc.SeoData = _mapper.Map<SeoData>(dtoLoc.SeoData);
                        loc.SeoData.AutoFillRelatedFields();
                    }
                }

                await _unitOfWork.LicenseTypesOfAggregator.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _unitOfWork.LicenseTypesOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<LicenseTypeOfAggregatorDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании типа лицензии. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<LicenseTypeOfAggregatorDetailDto> UpdateAsync(LicenseTypeOfAggregatorUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.CanonicalName ?? "", dto.Slug ?? "", dto.Id);

            var entity = await _unitOfWork.LicenseTypesOfAggregator.GetWithDescriptionsAndSeoAsync(dto.Id);
            if (entity == null) throw new NotFoundException("LicenseTypeOfAggregator", $"Тип лицензии с ID {dto.Id} не найден.");

            try
            {
                _mapper.Map(dto, entity);
                SyncLocalizationsWithSeo(entity, dto.Localizations);

                _unitOfWork.LicenseTypesOfAggregator.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.LicenseTypesOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<LicenseTypeOfAggregatorDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении типа лицензии ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.LicenseTypesOfAggregator.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _unitOfWork.LicenseTypesOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("LicenseTypeOfAggregator", $"Тип лицензии с ID {id} не найден.");

            _logger.LogWarning("Запущено ПОЛНОЕ УДАЛЕНИЕ типа лицензии ID {Id} ({Name}). CorrelationId: {CorrelationId}", 
                id, entity.CanonicalName, correlationId);

            try
            {
                // ЭТАП 1: Разрыв связей (если будут) - пока только сами локализации
                _logger.LogInformation("[Stage 1] Удаление локализаций и SEO данных для типа лицензии ID {Id}", id);
                foreach (var loc in entity.Localizations)
                {
                    if (loc.SeoDataId.HasValue)
                    {
                        await _unitOfWork.GetRepository<SeoData>().DeleteByIdAsync(loc.SeoDataId.Value);
                    }
                    _unitOfWork.GetRepository<LicenseTypeOfAggregatorLocalization>().Delete(loc);
                }
                
                await _unitOfWork.SaveChangesAsync();

                // ЭТАП 2: Удаление самого типа лицензии
                _logger.LogInformation("[Stage 2] Удаление корневой записи типа лицензии ID {Id}", id);
                _unitOfWork.LicenseTypesOfAggregator.Delete(entity);
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogCritical("Тип лицензии ID {Id} ПОЛНОСТЬЮ УДАЛЕН. CorrelationId: {CorrelationId}", id, correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при жестком удалении типа лицензии ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task RestoreAsync(int id)
        {
            await _unitOfWork.LicenseTypesOfAggregator.RestoreAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null) =>
            await _unitOfWork.LicenseTypesOfAggregator.IsCanonicalNameUniqueAsync(name, excludeId);

        public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null) =>
            await _unitOfWork.LicenseTypesOfAggregator.IsSlugUniqueAsync(slug, excludeId);

        public async Task<int> ClearAllAsync()
        {
            var correlationId = GetCorrelationId();
            try
            {
                var result = await _unitOfWork.TruncateTableAsync("license_types_of_aggregator");
                _unitOfWork.DetachAllEntities();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке таблицы типов лицензий. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<int> SeedFromJsonAsync()
        {
            try
            {
                return await _maintenanceSeeder.SeedAsync<LicenseTypeOfAggregatorCreateDto>(
                    "Pages/AGGREGATOR/LicenseTypeOfAggregator/Jsons/LicenseTypeOfAggregator.json",
                    async (dto) => {
                        if (await IsSlugUniqueAsync(dto.Slug))
                        {
                            await CreateAsync(dto);
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сидинге типов лицензий из JSON.");
                throw;
            }
        }

        private async Task CheckUniqueness(string name, string slug, int? excludeId = null)
        {
            if (!string.IsNullOrEmpty(name) && !await IsCanonicalNameUniqueAsync(name, excludeId))
                throw new ConflictException("Тип лицензии с таким каноническим именем уже существует.", "LicenseTypeOfAggregator", "CanonicalName");

            if (!string.IsNullOrEmpty(slug) && !await IsSlugUniqueAsync(slug, excludeId))
                throw new ConflictException("Тип лицензии с таким Slug уже существует.", "LicenseTypeOfAggregator", "Slug");
        }

        private void SyncLocalizationsWithSeo(LicenseEntity entity, List<LicenseTypeOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);

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
                        _unitOfWork.GetRepository<SeoData>().Delete(existing.SeoData);
                        existing.SeoData = null;
                        existing.SeoDataId = null;
                    }
                }
                else
                {
                    var newLoc = _mapper.Map<LicenseTypeOfAggregatorLocalization>(dto);
                    if (dto.SeoData != null)
                    {
                        newLoc.SeoData = _mapper.Map<SeoData>(dto.SeoData);
                        newLoc.SeoData.AutoFillRelatedFields();
                    }
                    entity.Localizations.Add(newLoc);
                }
            }

            var dtoLangIds = dtos.Select(d => d.LanguageOfAggregatorId).ToList();
            var toRemove = entity.Localizations.Where(l => !dtoLangIds.Contains(l.LanguageOfAggregatorId)).ToList();
            foreach (var remove in toRemove)
            {
                entity.Localizations.Remove(remove);
            }
        }

        private IQueryable<LicenseEntity> ApplySorting(IQueryable<LicenseEntity> query, LicenseTypeOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;
            
            return request.SortBy switch
            {
                LicenseTypeOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                LicenseTypeOfAggregatorSortField.CanonicalName => isAsc ? query.OrderBy(x => x.CanonicalName) : query.OrderByDescending(x => x.CanonicalName),
                LicenseTypeOfAggregatorSortField.Slug => isAsc ? query.OrderBy(x => x.Slug) : query.OrderByDescending(x => x.Slug),
                LicenseTypeOfAggregatorSortField.SortOrder => isAsc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                LicenseTypeOfAggregatorSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                LicenseTypeOfAggregatorSortField.UpdatedAt => isAsc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                _ => query.OrderBy(x => x.CanonicalName)
            };
        }
    }
}
