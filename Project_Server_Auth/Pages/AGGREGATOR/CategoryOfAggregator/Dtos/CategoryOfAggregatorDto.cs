using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos
{
    public class CategoryOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public class CategoryOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? LocalizedName { get; set; }
        public int ProgramsCount { get; set; }
        public int ChildrenCount { get; set; }
        
        // Поля для иерархического отображения (опционально)
        public int Level { get; set; }
        public List<CategoryOfAggregatorItemDto> Children { get; set; } = new();
    }

    public class CategoryOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public int SortOrder { get; set; }
        public List<CategoryOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class CategoryOfAggregatorCreateDto
    {
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSystem { get; set; } = false;
        public int SortOrder { get; set; } = 0;

        public List<CategoryOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    public class CategoryOfAggregatorUpdateDto : CategoryOfAggregatorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CategoryOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }
        public int? ParentId { get; set; }
        public CategoryOfAggregatorSortField SortBy { get; set; } = CategoryOfAggregatorSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
        public bool ShowDeleted { get; set; }
    }

    public class CategoryOfAggregatorPagedResponseDto
    {
        public IEnumerable<CategoryOfAggregatorItemDto> Items { get; set; } = new List<CategoryOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum CategoryOfAggregatorSortField
    {
        Id,
        CanonicalName,
        Slug,
        SortOrder,
        CreatedAt,
        ProgramsCount
    }
}
