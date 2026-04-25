using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Project_Server_Auth.Services.Maintenance.Interfaces;
using DeveloperEntity = DAL.Models.Aggregator.DeveloperOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Services
{
    public class DeveloperOfAggregatorService : IDeveloperOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeveloperOfAggregatorService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<DeveloperOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<DeveloperOfAggregatorUpdateDto> _updateValidator;
        private readonly IValidator<DeveloperOfAggregatorPageRequestDto> _pageRequestValidator;
        private readonly IMaintenanceSeeder _maintenanceSeeder;

        public DeveloperOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DeveloperOfAggregatorService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<DeveloperOfAggregatorCreateDto> createValidator,
            IValidator<DeveloperOfAggregatorUpdateDto> updateValidator,
            IValidator<DeveloperOfAggregatorPageRequestDto> pageRequestValidator,
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

        public async Task<DeveloperOfAggregatorPagedResponseDto> GetPagedAsync(
            DeveloperOfAggregatorPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.DevelopersOfAggregator.GetQueryable();

                if (request.ShowDeleted)
                {
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);
                }

                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x =>
                        x.Localizations.Any(l => l.LanguageOfAggregatorId == request.LanguageId.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(search) ||
                        x.SystemCode.ToLower().Contains(search) ||
                        x.Localizations.Any(l => l.Name.ToLower().Contains(search))
                    );
                }

                // Сортировка
                query = ApplySorting(query, request);

                var total = await query.CountAsync();

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations.Where(l =>
                        request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                    .ThenInclude(l => l.LanguageOfAggregator)
                    .Include(x => x.Programs) // Для подсчета ProgramsCount
                    .ToListAsync();

                var dtos = items.Select(item =>
                {
                    var dto = _mapper.Map<DeveloperOfAggregatorItemDto>(item);
                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId)
                              ?? item.Localizations.FirstOrDefault();
                    dto.LocalizedName = loc?.Name;
                    dto.ProgramsCount = item.Programs.Count;
                    return dto;
                });

                return new DeveloperOfAggregatorPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка разработчиков. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<DeveloperOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.DevelopersOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null)
                throw new NotFoundException("DeveloperOfAggregator", $"Разработчик с ID {id} не найден.");

            return _mapper.Map<DeveloperOfAggregatorDetailDto>(entity);
        }

        public async Task<DeveloperOfAggregatorDetailDto> CreateAsync(DeveloperOfAggregatorCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name, dto.SystemCode);

            try
            {
                var entity = _mapper.Map<DeveloperEntity>(dto);
                await _unitOfWork.DevelopersOfAggregator.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _unitOfWork.DevelopersOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<DeveloperOfAggregatorDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании разработчика. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<DeveloperOfAggregatorDetailDto> UpdateAsync(DeveloperOfAggregatorUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name ?? "", dto.SystemCode ?? "", dto.Id);

            var entity = await _unitOfWork.DevelopersOfAggregator.GetWithDescriptionsAndSeoAsync(dto.Id);
            if (entity == null)
                throw new NotFoundException("DeveloperOfAggregator", $"Разработчик с ID {dto.Id} не найден.");

            try
            {
                _mapper.Map(dto, entity);
                SyncLocalizationsWithSeo(entity, dto.Localizations);

                _unitOfWork.DevelopersOfAggregator.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.DevelopersOfAggregator.GetWithDescriptionsAndSeoAsync(entity.Id);
                return _mapper.Map<DeveloperOfAggregatorDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении разработчика ID {Id}. CorrelationId: {CorrelationId}",
                    dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.DevelopersOfAggregator.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _unitOfWork.DevelopersOfAggregator.GetWithDescriptionsAndSeoAsync(id);
            if (entity == null)
                throw new NotFoundException("DeveloperOfAggregator", $"Разработчик с ID {id} не найден.");

            _logger.LogWarning("Запущено ПОЛНОЕ УДАЛЕНИЕ разработчика ID {Id} ({Name}). CorrelationId: {CorrelationId}",
                id, entity.Name, correlationId);

            try
            {
                // ЭТАП 1: Разрыв связей с программами
                _logger.LogInformation("[Stage 1] Отключение разработчика от программ для ID {Id}", id);
                var programs = await _unitOfWork.GetRepository<DAL.Models.Aggregator.ProgramOfAggregator>()
                    .FindTrackingAsync(p => p.DeveloperOfAggregatorId == id);

                foreach (var program in programs)
                {
                    program.DeveloperOfAggregatorId = null;
                    _unitOfWork.GetRepository<DAL.Models.Aggregator.ProgramOfAggregator>().Update(program);
                }

                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("[Stage 1] Программы успешно отключены.");

                // ЭТАП 2: Удаление локализаций
                _logger.LogInformation("[Stage 2] Удаление локализаций для разработчика ID {Id}", id);
                foreach (var loc in entity.Localizations)
                {
                    _unitOfWork.GetRepository<DeveloperOfAggregatorLocalization>().Delete(loc);
                }

                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("[Stage 2] Локализации успешно удалены.");

                // ЭТАП 3: Удаление самого разработчика
                _logger.LogInformation("[Stage 3] Удаление корневой записи разработчика ID {Id}", id);
                _unitOfWork.DevelopersOfAggregator.Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogCritical("Разработчик ID {Id} ПОЛНОСТЬЮ УДАЛЕН ИЗ БАЗЫ. CorrelationId: {CorrelationId}", id,
                    correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ОШИБКА при жестком удалении разработчика ID {Id}. CorrelationId: {CorrelationId}",
                    id, correlationId);
                throw;
            }
        }

        public async Task RestoreAsync(int id)
        {
            var correlationId = GetCorrelationId();
            _logger.LogInformation("Восстановление разработчика ID {Id}. CorrelationId: {CorrelationId}", id,
                correlationId);
            await _unitOfWork.DevelopersOfAggregator.RestoreAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null) =>
            await _unitOfWork.DevelopersOfAggregator.IsNameUniqueAsync(name, excludeId);

        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null) =>
            await _unitOfWork.DevelopersOfAggregator.IsSystemCodeUniqueAsync(code, excludeId);

        public async Task<int> ClearAllAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.LogWarning("Запущена полная очистка таблицы разработчиков. CorrelationId: {CorrelationId}",
                correlationId);

            try
            {
                var result = await _unitOfWork.TruncateTableAsync("developers_of_aggregator");
                _unitOfWork.DetachAllEntities();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке таблицы разработчиков. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<int> SeedFromJsonAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.LogInformation("Запущен сидинг разработчиков из JSON. CorrelationId: {CorrelationId}",
                correlationId);

            try
            {
                return await _maintenanceSeeder.SeedAsync<DeveloperOfAggregatorCreateDto>(
                    "Pages/AGGREGATOR/DeveloperOfAggregator/Jsons/DeveloperOfAggregator.json",
                    async (dto) =>
                    {
                        if (await IsSystemCodeUniqueAsync(dto.SystemCode))
                        {
                            await CreateAsync(dto);
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сидинге разработчиков из JSON.");
                throw;
            }
        }

        private async Task CheckUniqueness(string name, string code, int? excludeId = null)
        {
            if (!string.IsNullOrEmpty(name) && !await IsNameUniqueAsync(name, excludeId))
                throw new ConflictException("Разработчик с таким именем уже существует.", "DeveloperOfAggregator",
                    "Name");

            if (!string.IsNullOrEmpty(code) && !await IsSystemCodeUniqueAsync(code, excludeId))
                throw new ConflictException("Разработчик с таким SystemCode уже существует.", "DeveloperOfAggregator",
                    "SystemCode");
        }

        private void SyncLocalizationsWithSeo(DeveloperEntity entity, List<DeveloperOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing =
                    entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                }
                else
                {
                    var newLoc = _mapper.Map<DeveloperOfAggregatorLocalization>(dto);
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

        private IQueryable<DeveloperEntity> ApplySorting(IQueryable<DeveloperEntity> query,
            DeveloperOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;

            return request.SortBy switch
            {
                DeveloperOfAggregatorSortField.Id => isAsc
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id),
                DeveloperOfAggregatorSortField.Name => isAsc
                    ? query.OrderBy(x => x.Name)
                    : query.OrderByDescending(x => x.Name),
                DeveloperOfAggregatorSortField.SystemCode => isAsc
                    ? query.OrderBy(x => x.SystemCode)
                    : query.OrderByDescending(x => x.SystemCode),
                DeveloperOfAggregatorSortField.Website => isAsc
                    ? query.OrderBy(x => x.Website)
                    : query.OrderByDescending(x => x.Website),
                DeveloperOfAggregatorSortField.SortOrder => isAsc
                    ? query.OrderBy(x => x.SortOrder)
                    : query.OrderByDescending(x => x.SortOrder),
                DeveloperOfAggregatorSortField.CreatedAt => isAsc
                    ? query.OrderBy(x => x.CreatedAt)
                    : query.OrderByDescending(x => x.CreatedAt),
                DeveloperOfAggregatorSortField.UpdatedAt => isAsc
                    ? query.OrderBy(x => x.UpdatedAt)
                    : query.OrderByDescending(x => x.UpdatedAt),
                DeveloperOfAggregatorSortField.ProgramsCount => isAsc
                    ? query.OrderBy(x => x.Programs.Count)
                    : query.OrderByDescending(x => x.Programs.Count),
                _ => query.OrderBy(x => x.Name)
            };
        }
    }
}