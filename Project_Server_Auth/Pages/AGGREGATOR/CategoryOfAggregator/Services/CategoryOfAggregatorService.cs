using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Interfaces;
using Project_Server_Auth.Services.Maintenance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using CategoryEntity = DAL.Models.Aggregator.CategoryOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Services
{
    public class CategoryOfAggregatorService : ICategoryOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryOfAggregatorService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<CategoryOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<CategoryOfAggregatorUpdateDto> _updateValidator;
        private readonly IValidator<CategoryOfAggregatorPageRequestDto> _pageRequestValidator;
        private readonly IMaintenanceSeeder _maintenanceSeeder;

        public CategoryOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryOfAggregatorService> logger,
            IHttpContextAccessor httpContextAccessor,
            IValidator<CategoryOfAggregatorCreateDto> createValidator,
            IValidator<CategoryOfAggregatorUpdateDto> updateValidator,
            IValidator<CategoryOfAggregatorPageRequestDto> pageRequestValidator,
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

        public async Task<CategoryOfAggregatorPagedResponseDto> GetPagedAsync(CategoryOfAggregatorPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.CategoriesOfAggregator.GetQueryable();

                if (request.ShowDeleted)
                {
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);
                }

                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x => x.Localizations.Any(l => l.LanguageOfAggregatorId == request.LanguageId.Value));
                }

                if (request.ParentId.HasValue)
                {
                    query = query.Where(x => x.ParentId == request.ParentId.Value);
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

                query = ApplySorting(query, request);

                var total = await query.CountAsync();

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                        .ThenInclude(l => l.LanguageOfAggregator)
                    .Include(x => x.Children)
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<CategoryOfAggregatorItemDto>(item);
                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId) 
                              ?? item.Localizations.FirstOrDefault();
                    dto.LocalizedName = loc?.Name;
                    // Count programs (Placeholder for now, assuming ProgramOfAggregator exists and has CategoryId)
                    // dto.ProgramsCount = ...
                    return dto;
                });

                return new CategoryOfAggregatorPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка категорий. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<CategoryOfAggregatorDetailDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.CategoriesOfAggregator.GetWithLocalizationsAsync(id);
            if (entity == null) throw new NotFoundException("CategoryOfAggregator", $"Категория с ID {id} не найдена.");

            return _mapper.Map<CategoryOfAggregatorDetailDto>(entity);
        }

        public async Task<CategoryOfAggregatorDetailDto> CreateAsync(CategoryOfAggregatorCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            if (!await _unitOfWork.CategoriesOfAggregator.IsCanonicalNameUniqueAsync(dto.CanonicalName))
                throw new ConflictException("Категория с таким техническим названием уже существует.", "CategoryOfAggregator", "CanonicalName");

            if (!await _unitOfWork.CategoriesOfAggregator.IsSlugUniqueAsync(dto.Slug))
                throw new ConflictException("Категория с таким Slug уже существует.", "CategoryOfAggregator", "Slug");

            try
            {
                var entity = _mapper.Map<CategoryEntity>(dto);
                await _unitOfWork.CategoriesOfAggregator.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var createdEntity = await _unitOfWork.CategoriesOfAggregator.GetWithLocalizationsAsync(entity.Id);
                return _mapper.Map<CategoryOfAggregatorDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<CategoryOfAggregatorDetailDto> UpdateAsync(CategoryOfAggregatorUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            if (!await _unitOfWork.CategoriesOfAggregator.IsCanonicalNameUniqueAsync(dto.CanonicalName, dto.Id))
                throw new ConflictException("Категория с таким техническим названием уже существует.", "CategoryOfAggregator", "CanonicalName");

            if (!await _unitOfWork.CategoriesOfAggregator.IsSlugUniqueAsync(dto.Slug, dto.Id))
                throw new ConflictException("Категория с таким Slug уже существует.", "CategoryOfAggregator", "Slug");

            var entity = await _unitOfWork.CategoriesOfAggregator.GetWithLocalizationsAsync(dto.Id);
            if (entity == null) throw new NotFoundException("CategoryOfAggregator", $"Категория с ID {dto.Id} не найдена.");

            try
            {
                _mapper.Map(dto, entity);
                SyncLocalizations(entity, dto.Localizations);

                _unitOfWork.CategoriesOfAggregator.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.CategoriesOfAggregator.GetWithLocalizationsAsync(entity.Id);
                return _mapper.Map<CategoryOfAggregatorDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении категории ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.CategoriesOfAggregator.SoftDeleteAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            var entity = await _unitOfWork.CategoriesOfAggregator.GetWithLocalizationsAsync(id);
            if (entity == null) throw new NotFoundException("CategoryOfAggregator", $"Категория с ID {id} не найдена.");

            // В реальной системе здесь нужно проверить наличие дочерних элементов или программ
            // Для упрощения просто удаляем

            foreach (var loc in entity.Localizations)
            {
                _unitOfWork.GetRepository<CategoryOfAggregatorLocalization>().Delete(loc);
            }

            _unitOfWork.CategoriesOfAggregator.Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            await _unitOfWork.CategoriesOfAggregator.RestoreAsync(id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<int> SeedFromJsonAsync()
        {
            var correlationId = GetCorrelationId();
            var filePath = Path.Combine(AppContext.BaseDirectory, "Pages/AGGREGATOR/CategoryOfAggregator/Jsons/CategoryOfAggregator.json");
            
            // Если не нашли в bin, ищем в ContentRoot
            if (!File.Exists(filePath))
            {
                var env = (IWebHostEnvironment)_httpContextAccessor.HttpContext?.RequestServices.GetService(typeof(IWebHostEnvironment))!;
                filePath = Path.Combine(env.ContentRootPath, "Pages/AGGREGATOR/CategoryOfAggregator/Jsons/CategoryOfAggregator.json");
            }

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл данных для сидинга не найден.", filePath);

            var json = await File.ReadAllTextAsync(filePath);
            var seedDtos = JsonSerializer.Deserialize<List<CategorySeedDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (seedDtos == null || !seedDtos.Any()) return 0;

            var idMap = new Dictionary<int, int>(); // JsonID -> DbID

            // Шаг 1: Создаем все категории без ParentId
            foreach (var sDto in seedDtos)
            {
                if (await _unitOfWork.CategoriesOfAggregator.IsCanonicalNameUniqueAsync(sDto.CanonicalName))
                {
                    var entity = new CategoryEntity
                    {
                        CanonicalName = sDto.CanonicalName,
                        Slug = sDto.Slug,
                        IconPath = sDto.IconPath,
                        IsActive = sDto.IsActive,
                        IsSystem = sDto.IsSystem,
                        SortOrder = sDto.SortOrder
                    };

                    foreach (var lDto in sDto.Localizations)
                    {
                        entity.Localizations.Add(new CategoryOfAggregatorLocalization
                        {
                            LanguageOfAggregatorId = lDto.LanguageOfAggregatorId,
                            Name = lDto.Name,
                            Description = lDto.Description,
                            MetaTitle = lDto.MetaTitle,
                            MetaDescription = lDto.MetaDescription
                        });
                    }

                    await _unitOfWork.CategoriesOfAggregator.AddAsync(entity);
                    await _unitOfWork.SaveChangesAsync();
                    idMap[sDto.Id] = entity.Id;
                }
                else
                {
                    // Если уже есть, находим существующий и обновляем поля
                    var existing = await _unitOfWork.CategoriesOfAggregator.GetQueryable()
                        .FirstOrDefaultAsync(x => x.CanonicalName == sDto.CanonicalName);
                    
                    if (existing != null)
                    {
                        idMap[sDto.Id] = existing.Id;

                        bool changed = false;
                        if (existing.IconPath != sDto.IconPath) { existing.IconPath = sDto.IconPath; changed = true; }
                        if (existing.SortOrder != sDto.SortOrder) { existing.SortOrder = sDto.SortOrder; changed = true; }
                        if (existing.Slug != sDto.Slug) { existing.Slug = sDto.Slug; changed = true; }
                        if (existing.IsActive != sDto.IsActive) { existing.IsActive = sDto.IsActive; changed = true; }
                        if (existing.IsSystem != sDto.IsSystem) { existing.IsSystem = sDto.IsSystem; changed = true; }

                        if (changed)
                        {
                            _unitOfWork.CategoriesOfAggregator.Update(existing);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }
            }

            // Шаг 2: Проставляем ParentId
            int updatedCount = 0;
            foreach (var sDto in seedDtos)
            {
                if (sDto.ParentId.HasValue && idMap.TryGetValue(sDto.ParentId.Value, out int dbParentId))
                {
                    if (idMap.TryGetValue(sDto.Id, out int dbId))
                    {
                        var entity = await _unitOfWork.CategoriesOfAggregator.GetByIdAsync(dbId);
                        if (entity != null && entity.ParentId != dbParentId)
                        {
                            entity.ParentId = dbParentId;
                            _unitOfWork.CategoriesOfAggregator.Update(entity);
                            updatedCount++;
                        }
                    }
                }
            }

            if (updatedCount > 0) await _unitOfWork.SaveChangesAsync();

            return seedDtos.Count;
        }

        // Вспомогательный класс для десериализации сидинга
        private class CategorySeedDto
        {
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string CanonicalName { get; set; } = string.Empty;
            public string Slug { get; set; } = string.Empty;
            public string? IconPath { get; set; }
            public bool IsActive { get; set; }
            public bool IsSystem { get; set; }
            public int SortOrder { get; set; }
            public List<CategoryOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        }

        public async Task<bool> ClearAllAsync()
        {
            await _unitOfWork.TruncateTableAsync("categories_of_aggregator");
            _unitOfWork.DetachAllEntities();
            return true;
        }

        public async Task<List<CategoryOfAggregatorItemDto>> GetTreeAsync(int? languageId = null)
        {
            var allItems = await _unitOfWork.CategoriesOfAggregator.GetTreeAsync(languageId);
            
            var dtos = allItems.Select(item => {
                var dto = _mapper.Map<CategoryOfAggregatorItemDto>(item);
                var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == languageId) 
                          ?? item.Localizations.FirstOrDefault();
                dto.LocalizedName = loc?.Name;
                return dto;
            }).ToList();

            // Построение дерева
            var rootNodes = dtos.Where(d => d.ParentId == null).ToList();
            foreach (var root in rootNodes)
            {
                BuildTree(root, dtos, 0);
            }

            return rootNodes;
        }

        private void BuildTree(CategoryOfAggregatorItemDto parent, List<CategoryOfAggregatorItemDto> all, int level)
        {
            parent.Level = level;
            parent.Children = all.Where(x => x.ParentId == parent.Id).ToList();
            foreach (var child in parent.Children)
            {
                BuildTree(child, all, level + 1);
            }
        }

        private void SyncLocalizations(CategoryEntity entity, List<CategoryOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                }
                else
                {
                    var newLoc = _mapper.Map<CategoryOfAggregatorLocalization>(dto);
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

        private IQueryable<CategoryEntity> ApplySorting(IQueryable<CategoryEntity> query, CategoryOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;
            
            return request.SortBy switch
            {
                CategoryOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                CategoryOfAggregatorSortField.CanonicalName => isAsc ? query.OrderBy(x => x.CanonicalName) : query.OrderByDescending(x => x.CanonicalName),
                CategoryOfAggregatorSortField.Slug => isAsc ? query.OrderBy(x => x.Slug) : query.OrderByDescending(x => x.Slug),
                CategoryOfAggregatorSortField.SortOrder => isAsc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                CategoryOfAggregatorSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.SortOrder)
            };
        }
    }
}
