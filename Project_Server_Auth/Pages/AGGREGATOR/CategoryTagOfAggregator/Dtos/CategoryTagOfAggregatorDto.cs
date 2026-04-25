using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos
{
    public class CategoryTagOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class CategoryTagOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string? LocalizedName { get; set; }
        public int TagsCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class CategoryTagOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<CategoryTagOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class CategoryTagOfAggregatorCreateDto
    {
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? IconPath { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public List<CategoryTagOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    public class CategoryTagOfAggregatorUpdateDto : CategoryTagOfAggregatorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CategoryTagOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }
        public CategoryTagOfAggregatorSortField SortBy { get; set; } = CategoryTagOfAggregatorSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
        public bool ShowDeleted { get; set; }
    }

    public class CategoryTagOfAggregatorPagedResponseDto
    {
        public IEnumerable<CategoryTagOfAggregatorItemDto> Items { get; set; } = new List<CategoryTagOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum CategoryTagOfAggregatorSortField
    {
        Id,
        Slug,
        SortOrder,
        CreatedAt,
        UpdatedAt,
        TagsCount
    }
}
