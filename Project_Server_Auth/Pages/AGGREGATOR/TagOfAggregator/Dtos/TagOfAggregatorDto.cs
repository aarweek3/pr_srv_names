using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Aggregator.Enums;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos
{
    public class TagOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? H1Title { get; set; }
    }

    public class TagOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int CategoryTagId { get; set; }
        public string? CategoryName { get; set; }
        
        public TagType Type { get; set; }
        public string Color { get; set; } = "inherit";
        public string? IconPath { get; set; }
        public bool IsFeature { get; set; }
        
        /// <summary>
        /// Итоговый цвет (с учетом наследования от категории).
        /// </summary>
        public string? DisplayColor { get; set; }

        /// <summary>
        /// Итоговый путь к иконке (с учетом наследования от категории).
        /// </summary>
        public string? DisplayIcon { get; set; }

        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string? LocalizedName { get; set; }
        public bool RequiresTranslation { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class TagOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int CategoryTagId { get; set; }
        public TagType Type { get; set; }
        public string Color { get; set; } = "inherit";
        public string? IconPath { get; set; }
        public bool IsFeature { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public List<TagOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class TagOfAggregatorCreateDto
    {
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public int CategoryTagId { get; set; }
        public TagType Type { get; set; } = TagType.Functional;

        [MaxLength(50)]
        public string Color { get; set; } = "inherit";

        [MaxLength(255)]
        public string? IconPath { get; set; }

        public bool IsFeature { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public List<TagOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    public class TagOfAggregatorUpdateDto : TagOfAggregatorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class TagOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? CategoryTagId { get; set; }
        public int? LanguageId { get; set; }
        public TagOfAggregatorSortField SortBy { get; set; } = TagOfAggregatorSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
        public bool ShowDeleted { get; set; }
    }

    public class TagOfAggregatorPagedResponseDto
    {
        public IEnumerable<TagOfAggregatorItemDto> Items { get; set; } = new List<TagOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum TagOfAggregatorSortField
    {
        Id,
        Slug,
        SortOrder,
        CreatedAt,
        UpdatedAt,
        Type
    }
}
