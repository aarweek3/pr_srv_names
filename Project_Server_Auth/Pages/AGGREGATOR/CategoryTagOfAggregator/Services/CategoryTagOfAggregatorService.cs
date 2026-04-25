using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_Server_Auth.Services.Maintenance.Interfaces;
using FluentValidation;
using DAL.Models.Aggregator.Enums;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Services
{
    public class CategoryTagOfAggregatorService : ICategoryTagOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryTagOfAggregatorService> _logger;
        private readonly IMaintenanceSeeder _maintenanceSeeder;
        private readonly IValidator<CategoryTagOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<CategoryTagOfAggregatorUpdateDto> _updateValidator;

        public CategoryTagOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryTagOfAggregatorService> logger,
            IMaintenanceSeeder maintenanceSeeder,
            IValidator<CategoryTagOfAggregatorCreateDto> createValidator,
            IValidator<CategoryTagOfAggregatorUpdateDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _maintenanceSeeder = maintenanceSeeder;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CategoryTagOfAggregatorPagedResponseDto> GetPagedAsync(CategoryTagOfAggregatorPageRequestDto request)
        {
            try
            {
                var query = _unitOfWork.CategoryTags.GetQueryable();

                if (request.ShowDeleted)
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x => 
                        x.Slug.ToLower().Contains(search) || 
                        x.Localizations.Any(l => l.Name.ToLower().Contains(search))
                    );
                }

                query = ApplySorting(query, request);

                var total = await query.CountAsync();
                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations)
                        .ThenInclude(l => l.LanguageOfAggregator)
                    .Include(x => x.Tags)
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<CategoryTagOfAggregatorItemDto>(item);
                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId) 
                              ?? item.Localizations.FirstOrDefault();
                    dto.LocalizedName = loc?.Name;
                    dto.TagsCount = item.Tags.Count;
                    return dto;
                });

                return new CategoryTagOfAggregatorPagedResponseDto {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged category tags");
                throw;
            }
        }

        public async Task<CategoryTagOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.CategoryTags.GetQueryable()
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) throw new NotFoundException("CategoryTagOfAggregator", id.ToString());

            return _mapper.Map<CategoryTagOfAggregatorDetailDto>(entity);
        }

        public async Task<CategoryTagOfAggregatorDetailDto> CreateAsync(CategoryTagOfAggregatorCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Slug);

            var entity = _mapper.Map<DAL.Models.Aggregator.CategoryTagOfAggregator>(dto);
            SyncLocalizations(entity, dto.Localizations);

            await _unitOfWork.CategoryTags.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryTagOfAggregatorDetailDto>(entity);
        }

        public async Task<CategoryTagOfAggregatorDetailDto> UpdateAsync(CategoryTagOfAggregatorUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Slug, dto.Id);

            var entity = await _unitOfWork.CategoryTags.GetQueryable()
                .Include(x => x.Localizations)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null) throw new NotFoundException("CategoryTagOfAggregator", dto.Id.ToString());

            _mapper.Map(dto, entity);
            SyncLocalizations(entity, dto.Localizations);

            _unitOfWork.CategoryTags.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryTagOfAggregatorDetailDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var hasTags = await _unitOfWork.Tags.GetQueryable()
                .AnyAsync(t => t.CategoryTagId == id);

            if (hasTags)
                throw new ConflictException("Нельзя удалить категорию, в которой есть теги. Сначала удалите или переместите теги.", "CategoryTagOfAggregator", "Tags");

            await _unitOfWork.CategoryTags.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            await _unitOfWork.CategoryTags.RestoreAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task CheckUniqueness(string slug, int? excludeId = null)
        {
            var exists = await _unitOfWork.CategoryTags.GetQueryable()
                .AnyAsync(x => x.Slug == slug && x.Id != excludeId);

            if (exists)
                throw new ConflictException("Группа тегов с таким Slug уже существует.", "CategoryTagOfAggregator", "Slug");
        }

        public async Task<int> ClearAllAsync()
        {
            _logger.LogWarning("Запущена полная очистка таблицы категорий тегов.");
            var result = await _unitOfWork.TruncateTableAsync("category_tags_of_aggregator");
            _unitOfWork.DetachAllEntities();
            return result;
        }

        public async Task<int> SeedFromJsonAsync()
        {
            _logger.LogInformation("Запущен сидинг категорий тегов из JSON.");
            
            // Загружаем маппинг языков один раз для ускорения
            var languages = await _unitOfWork.LanguagesOfAggregator.GetQueryable().ToListAsync();

            return await _maintenanceSeeder.SeedAsync<CategoryTagSeedDto>(
                "Pages/AGGREGATOR/TagOfAggregator/Jsons/tags-seed.json",
                async (seedDto) => {
                    // Проверяем уникальность категории
                    if (!await _unitOfWork.CategoryTags.IsSlugUniqueAsync(seedDto.Slug)) return;

                    var createDto = new CategoryTagOfAggregatorCreateDto
                    {
                        Slug = seedDto.Slug,
                        IconPath = seedDto.IconName,
                        Color = seedDto.Color,
                        SortOrder = seedDto.SortOrder,
                        IsActive = seedDto.IsActive,
                        Localizations = new List<CategoryTagOfAggregatorLocalizationDto>()
                    };

                    // Маппинг локализаций категории
                    foreach (var trans in seedDto.Translations)
                    {
                        var lang = languages.FirstOrDefault(l => l.Code == trans.Key);
                        if (lang != null)
                        {
                            createDto.Localizations.Add(new CategoryTagOfAggregatorLocalizationDto
                            {
                                LanguageOfAggregatorId = lang.Id,
                                Name = trans.Value.Name,
                                Description = trans.Value.Description
                            });
                        }
                    }

                    // Создаем категорию
                    var category = _mapper.Map<DAL.Models.Aggregator.CategoryTagOfAggregator>(createDto);
                    SyncLocalizations(category, createDto.Localizations);
                    await _unitOfWork.CategoryTags.AddAsync(category);
                    await _unitOfWork.SaveChangesAsync();

                    // Сидинг тегов внутри категории
                    if (seedDto.Tags != null)
                    {
                        foreach (var tagSeed in seedDto.Tags)
                        {
                            if (!await _unitOfWork.Tags.IsSlugUniqueAsync(tagSeed.Slug)) continue;

                            var tagEntity = new DAL.Models.Aggregator.TagOfAggregator
                            {
                                Slug = tagSeed.Slug,
                                CategoryTagId = category.Id,
                                SortOrder = tagSeed.SortOrder,
                                IsFeature = tagSeed.IsFeature,
                                Type = Enum.TryParse<TagType>(tagSeed.Type, true, out var t) ? t : TagType.Functional,
                                Color = tagSeed.Color ?? "inherit",
                                IconPath = tagSeed.IconName ?? "inherit",
                                IsActive = true,
                                Localizations = new List<TagOfAggregatorLocalization>()
                            };

                            foreach (var tagTrans in tagSeed.Translations)
                            {
                                var lang = languages.FirstOrDefault(l => l.Code == tagTrans.Key);
                                if (lang != null)
                                {
                                    tagEntity.Localizations.Add(new TagOfAggregatorLocalization
                                    {
                                        LanguageOfAggregatorId = lang.Id,
                                        Name = tagTrans.Value.Name,
                                        H1Title = tagTrans.Value.H1Title,
                                        MetaTitle = tagTrans.Value.MetaTitle,
                                        MetaDescription = tagTrans.Value.MetaDescription
                                    });
                                }
                            }

                            await _unitOfWork.Tags.AddAsync(tagEntity);
                        }
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
            );
        }

        private void SyncLocalizations(DAL.Models.Aggregator.CategoryTagOfAggregator entity, List<CategoryTagOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                    _mapper.Map(dto, existing);
                else
                    entity.Localizations.Add(_mapper.Map<CategoryTagOfAggregatorLocalization>(dto));
            }

            var dtoLangIds = dtos.Select(d => d.LanguageOfAggregatorId).ToList();
            var toRemove = entity.Localizations.Where(l => !dtoLangIds.Contains(l.LanguageOfAggregatorId)).ToList();
            foreach (var remove in toRemove)
                entity.Localizations.Remove(remove);
        }

        private IQueryable<DAL.Models.Aggregator.CategoryTagOfAggregator> ApplySorting(IQueryable<DAL.Models.Aggregator.CategoryTagOfAggregator> query, CategoryTagOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;
            
            return request.SortBy switch
            {
                CategoryTagOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                CategoryTagOfAggregatorSortField.Slug => isAsc ? query.OrderBy(x => x.Slug) : query.OrderByDescending(x => x.Slug),
                CategoryTagOfAggregatorSortField.SortOrder => isAsc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                CategoryTagOfAggregatorSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                CategoryTagOfAggregatorSortField.UpdatedAt => isAsc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                CategoryTagOfAggregatorSortField.TagsCount => isAsc ? query.OrderBy(x => x.Tags.Count) : query.OrderByDescending(x => x.Tags.Count),
                _ => query.OrderBy(x => x.SortOrder)
            };
        }
    }
}
