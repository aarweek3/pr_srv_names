using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using DAL.Models.GeneralModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Interfaces;
using pr_srv_names.Pages.Shared.Seo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Project_Server_Auth.Services.Maintenance.Interfaces;
using PlatformEntity = DAL.Models.Aggregator.PlatformOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Services
{
    public class PlatformOfAggregatorService : IPlatformOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformOfAggregatorService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<PlatformOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<PlatformOfAggregatorUpdateDto> _updateValidator;
        private readonly IValidator<PlatformOfAggregatorPageRequestDto> _pageRequestValidator;
        private readonly IMaintenanceSeeder _maintenanceSeeder;

        public PlatformOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PlatformOfAggregatorService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<PlatformOfAggregatorCreateDto> createValidator,
            IValidator<PlatformOfAggregatorUpdateDto> updateValidator,
            IValidator<PlatformOfAggregatorPageRequestDto> pageRequestValidator,
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

        public async Task<PlatformOfAggregatorPagedResponseDto> GetPagedAsync(PlatformOfAggregatorPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.PlatformsOfAggregator.GetQueryable();

                // Обработка Soft Delete
                if (request.ShowDeleted)
                {
                    // Для режима корзины показываем ТОЛЬКО удаленные
                    // Игнорируем глобальный Query Filter
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);
                }

                // Фильтрация по языку (если указан)
                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x => x.Localizations.Any(l => l.LanguageOfAggregatorId == request.LanguageId.Value));
                }

                // Поиск по названию или коду
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x => 
                        x.Name.ToLower().Contains(search) || 
                        x.SystemCode.ToLower().Contains(search) ||
                        x.Localizations.Any(l => l.Name.ToLower().Contains(search))
                    );
                }

                var total = await query.CountAsync();

                // Сортировка
                query = ApplySorting(query, request);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                        .ThenInclude(l => l.LanguageOfAggregator)
                    .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                        .ThenInclude(l => l.SeoData)
                    .Include(x => x.ProgramPlatforms) // Для подсчета ProgramsCount
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<PlatformOfAggregatorItemDto>(item);
                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId) 
                              ?? item.Localizations.FirstOrDefault();
                    dto.LocalizedName = loc?.Name;
                    return dto;
                });

                return new PlatformOfAggregatorPagedResponseDto
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

        public async Task<PlatformOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.PlatformsOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("PlatformOfAggregator", $"Платформа с ID {id} не найдена.");

            return _mapper.Map<PlatformOfAggregatorDetailDto>(entity);
        }

        public async Task<PlatformOfAggregatorDetailDto> CreateAsync(PlatformOfAggregatorCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name, dto.SystemCode);

            try
            {
                var entity = _mapper.Map<PlatformEntity>(dto);
                
                // Обработка SEO-данных в локализациях
                foreach (var loc in entity.Localizations)
                {
                    var dtoLoc = dto.Localizations.FirstOrDefault(d => d.LanguageOfAggregatorId == loc.LanguageOfAggregatorId);
                    if (dtoLoc?.SeoData != null)
                    {
                        loc.SeoData = _mapper.Map<SeoData>(dtoLoc.SeoData);
                        loc.SeoData.AutoFillRelatedFields();
                    }
                }

                await _unitOfWork.PlatformsOfAggregator.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _unitOfWork.PlatformsOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<PlatformOfAggregatorDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании платформы. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<PlatformOfAggregatorDetailDto> UpdateAsync(PlatformOfAggregatorUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name ?? "", dto.SystemCode ?? "", dto.Id);

            var entity = await _unitOfWork.PlatformsOfAggregator.GetWithDescriptionsAndSeoAsync(dto.Id);
            if (entity == null) throw new NotFoundException("PlatformOfAggregator", $"Платформа с ID {dto.Id} не найдена.");

            try
            {
                _mapper.Map(dto, entity);
                SyncLocalizationsWithSeo(entity, dto.Localizations);

                _unitOfWork.PlatformsOfAggregator.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.PlatformsOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<PlatformOfAggregatorDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении платформы ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.PlatformsOfAggregator.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _unitOfWork.PlatformsOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null) throw new NotFoundException("PlatformOfAggregator", $"Платформа с ID {id} не найдена.");

            _logger.LogWarning("Запущено ПОЛНОЕ УДАЛЕНИЕ платформы ID {Id} ({PlatformName}). CorrelationId: {CorrelationId}", 
                id, entity.Name, correlationId);

            try
            {
                // ЭТАП 1: Разрыв связей с программами
                // Это самый критичный момент из-за Restrict ограничений в БД
                _logger.LogInformation("[Stage 1] Удаление связей с программами для платформы ID {Id}", id);
                await _unitOfWork.GetRepository<ProgramPlatformOfAggregator>()
                    .DeleteManyAsync(pp => pp.PlatformOfAggregatorId == id);
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("[Stage 1] Связи успешно удалены.");

                // ЭТАП 2: Удаление локализаций и SEO
                _logger.LogInformation("[Stage 2] Удаление локализаций и SEO данных для платформы ID {Id}", id);
                foreach (var loc in entity.Localizations)
                {
                    if (loc.SeoDataId.HasValue)
                    {
                        await _unitOfWork.GetRepository<SeoData>().DeleteByIdAsync(loc.SeoDataId.Value);
                    }
                    _unitOfWork.GetRepository<PlatformOfAggregatorLocalization>().Delete(loc);
                }
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("[Stage 2] Локализации успешно удалены.");

                // ЭТАП 3: Удаление самой платформы
                _logger.LogInformation("[Stage 3] Удаление корневой записи платформы ID {Id}", id);
                _unitOfWork.PlatformsOfAggregator.Delete(entity);
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("[Stage 3] Платформа успешно удалена из БД.");

                _logger.LogCritical("Платформа ID {Id} ПОЛНОСТЬЮ УДАЛЕНА ИЗ БАЗЫ. CorrelationId: {CorrelationId}", id, correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ОШИБКА при жестком удалении платформы ID {Id} на одном из этапов. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task RestoreAsync(int id)
        {
            var correlationId = GetCorrelationId();
            _logger.LogInformation("Восстановление платформы ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
            
            await _unitOfWork.PlatformsOfAggregator.RestoreAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null) =>
            await _unitOfWork.PlatformsOfAggregator.IsNameUniqueAsync(name, excludeId);

        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null) =>
            await _unitOfWork.PlatformsOfAggregator.IsSystemCodeUniqueAsync(code, excludeId);

        public async Task<int> ClearAllAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.LogWarning("Запущена полная очистка таблицы платформ агрегатора. CorrelationId: {CorrelationId}", correlationId);

            try
            {
                // Используем стандартный хелпер для PostgreSQL (CASCADE + RESTART IDENTITY)
                var result = await _unitOfWork.TruncateTableAsync("platforms_of_aggregator");
                _unitOfWork.DetachAllEntities();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке таблицы платформ. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<int> SeedFromJsonAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.LogInformation("Запущен сидинг платформ из JSON. CorrelationId: {CorrelationId}", correlationId);

            try
            {
                // Используем универсальный Seeder
                return await _maintenanceSeeder.SeedAsync<PlatformOfAggregatorCreateDto>(
                    "Pages/AGGREGATOR/PlatformOfAggregator/Jsons/PlatformOfAgregator.json",
                    async (dto) => {
                        if (await IsSystemCodeUniqueAsync(dto.SystemCode))
                        {
                            await CreateAsync(dto);
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сидинге платформ из JSON. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        private async Task CheckUniqueness(string name, string systemCode, int? excludeId = null)
        {
            if (!string.IsNullOrEmpty(name) && !await IsNameUniqueAsync(name, excludeId))
                throw new ConflictException("Платформа с таким техническим именем уже существует.", "PlatformOfAggregator", "Name");

            if (!string.IsNullOrEmpty(systemCode) && !await IsSystemCodeUniqueAsync(systemCode, excludeId))
                throw new ConflictException("Платформа с таким системным кодом уже существует.", "PlatformOfAggregator", "SystemCode");
        }

        private void SyncLocalizationsWithSeo(PlatformEntity entity, List<PlatformOfAggregatorLocalizationDto> dtos)
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
                    var newLoc = _mapper.Map<PlatformOfAggregatorLocalization>(dto);
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

        private IQueryable<PlatformEntity> ApplySorting(IQueryable<PlatformEntity> query, PlatformOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;
            
            return request.SortBy switch
            {
                PlatformOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                PlatformOfAggregatorSortField.Name => isAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                PlatformOfAggregatorSortField.SystemCode => isAsc ? query.OrderBy(x => x.SystemCode) : query.OrderByDescending(x => x.SystemCode),
                PlatformOfAggregatorSortField.SortOrder => isAsc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                PlatformOfAggregatorSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                PlatformOfAggregatorSortField.UpdatedAt => isAsc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                PlatformOfAggregatorSortField.ProgramsCount => isAsc ? query.OrderBy(x => x.ProgramPlatforms.Count) : query.OrderByDescending(x => x.ProgramPlatforms.Count),
                _ => query.OrderBy(x => x.Name)
            };
        }
    }
}
