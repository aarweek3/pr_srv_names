using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Services
{
    public class TagOfAggregatorService : ITagOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TagOfAggregatorService> _logger;
        private readonly IValidator<TagOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<TagOfAggregatorUpdateDto> _updateValidator;
        private readonly ICategoryTagOfAggregatorService _categoryService;

        public TagOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<TagOfAggregatorService> logger,
            IValidator<TagOfAggregatorCreateDto> createValidator,
            IValidator<TagOfAggregatorUpdateDto> updateValidator,
            ICategoryTagOfAggregatorService categoryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _categoryService = categoryService;
        }

        public async Task<TagOfAggregatorPagedResponseDto> GetPagedAsync(TagOfAggregatorPageRequestDto request)
        {
            try
            {
                var query = _unitOfWork.Tags.GetQueryable();

                if (request.ShowDeleted)
                    query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);

                if (request.CategoryTagId.HasValue)
                    query = query.Where(x => x.CategoryTagId == request.CategoryTagId.Value);

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    query = query.Where(x =>
                        x.Slug.ToLower().Contains(search) ||
                        x.Localizations.Any(l => l.Name.ToLower().Contains(search))
                    );
                }

                // Включаем Категорию для корректного маппинга DisplayColor/DisplayIcon
                query = query.Include(x => x.Category);

                query = ApplySorting(query, request);

                var total = await query.CountAsync();

                // Получаем ID системного языка для проверки RequiresTranslation
                var defaultLang = await _unitOfWork.LanguagesOfAggregator.GetQueryable()
                    .FirstOrDefaultAsync(l => l.IsDefault);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Localizations)
                        .ThenInclude(l => l.LanguageOfAggregator)
                    .ToListAsync();

                var dtos = items.Select(item =>
                {
                    var dto = _mapper.Map<TagOfAggregatorItemDto>(item);

                    var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId)
                              ?? item.Localizations.FirstOrDefault();

                    dto.LocalizedName = loc?.Name;

                    // Проверка: нужен ли перевод (отсутствие локализации для системного языка)
                    if (defaultLang != null)
                    {
                        dto.RequiresTranslation = !item.Localizations.Any(l => l.LanguageOfAggregatorId == defaultLang.Id);
                    }

                    return dto;
                });

                return new TagOfAggregatorPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged tags");
                throw;
            }
        }

        public async Task<TagOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Tags.GetQueryable()
                .Include(x => x.Category)
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) throw new NotFoundException("TagOfAggregator", id.ToString());

            return _mapper.Map<TagOfAggregatorDetailDto>(entity);
        }

        public async Task<TagOfAggregatorDetailDto> CreateAsync(TagOfAggregatorCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Slug);

            var entity = _mapper.Map<DAL.Models.Aggregator.TagOfAggregator>(dto);
            SyncLocalizations(entity, dto.Localizations);

            await _unitOfWork.Tags.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // Перечитываем с инклюдами для возврата полного DTO
            return (await GetByIdAsync(entity.Id))!;
        }

        public async Task<TagOfAggregatorDetailDto> UpdateAsync(TagOfAggregatorUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Slug, dto.Id);

            var entity = await _unitOfWork.Tags.GetQueryable()
                .Include(x => x.Localizations)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null) throw new NotFoundException("TagOfAggregator", dto.Id.ToString());

            _mapper.Map(dto, entity);
            SyncLocalizations(entity, dto.Localizations);

            _unitOfWork.Tags.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return (await GetByIdAsync(entity.Id))!;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Tags.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            await _unitOfWork.Tags.RestoreAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateSortOrderAsync(int id, int newSortOrder)
        {
            var entity = await _unitOfWork.Tags.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException("TagOfAggregator", id.ToString());

            entity.SortOrder = newSortOrder;
            _unitOfWork.Tags.Update(entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        private async Task CheckUniqueness(string slug, int? excludeId = null)
        {
            var exists = await _unitOfWork.Tags.GetQueryable()
                .AnyAsync(x => x.Slug == slug && x.Id != excludeId);

            if (exists)
                throw new ConflictException("Тег с таким Slug уже существует.", "TagOfAggregator", "Slug");
        }

        public async Task<int> ClearAllAsync()
        {
            _logger.LogWarning("Запущена полная очистка таблицы тегов.");
            var result = await _unitOfWork.TruncateTableAsync("tags_of_aggregator");
            _unitOfWork.DetachAllEntities();
            return result;
        }

        public async Task<int> SeedFromJsonAsync()
        {
            // Теги сидятся через категории, так как они вложены в JSON
            // Но для стандарта мы вызываем сидинг через сервис категорий
            return await _categoryService.SeedFromJsonAsync();
        }

        private void SyncLocalizations(DAL.Models.Aggregator.TagOfAggregator entity, List<TagOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                    _mapper.Map(dto, existing);
                else
                    entity.Localizations.Add(_mapper.Map<TagOfAggregatorLocalization>(dto));
            }

            var dtoLangIds = dtos.Select(d => d.LanguageOfAggregatorId).ToList();
            var toRemove = entity.Localizations.Where(l => !dtoLangIds.Contains(l.LanguageOfAggregatorId)).ToList();
            foreach (var remove in toRemove)
                entity.Localizations.Remove(remove);
        }

        private IQueryable<DAL.Models.Aggregator.TagOfAggregator> ApplySorting(IQueryable<DAL.Models.Aggregator.TagOfAggregator> query, TagOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;

            return request.SortBy switch
            {
                TagOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                TagOfAggregatorSortField.Slug => isAsc ? query.OrderBy(x => x.Slug) : query.OrderByDescending(x => x.Slug),
                TagOfAggregatorSortField.SortOrder => isAsc ? query.OrderBy(x => x.SortOrder) : query.OrderByDescending(x => x.SortOrder),
                TagOfAggregatorSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                TagOfAggregatorSortField.UpdatedAt => isAsc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                TagOfAggregatorSortField.Type => isAsc ? query.OrderBy(x => x.Type) : query.OrderByDescending(x => x.Type),
                _ => query.OrderBy(x => x.SortOrder)
            };
        }
    }
}
