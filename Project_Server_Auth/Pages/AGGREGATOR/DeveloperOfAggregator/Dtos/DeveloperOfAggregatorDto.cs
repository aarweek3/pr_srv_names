using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos
{
    public class DeveloperOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        
        // SEO поля напрямую в локализации (согласно модели)
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public class DeveloperOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SystemCode { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? LocalizedName { get; set; }
        public int ProgramsCount { get; set; }
    }

    public class DeveloperOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SystemCode { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<DeveloperOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class DeveloperOfAggregatorCreateDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string SystemCode { get; set; } = string.Empty;

        public string? Website { get; set; }
        public string? IconPath { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public List<DeveloperOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    public class DeveloperOfAggregatorUpdateDto : DeveloperOfAggregatorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class DeveloperOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }
        public DeveloperOfAggregatorSortField SortBy { get; set; } = DeveloperOfAggregatorSortField.Name;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
        public bool ShowDeleted { get; set; }
    }

    public class DeveloperOfAggregatorPagedResponseDto
    {
        public IEnumerable<DeveloperOfAggregatorItemDto> Items { get; set; } = new List<DeveloperOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum DeveloperOfAggregatorSortField
    {
        Id,
        Name,
        SystemCode,
        Website,
        SortOrder,
        CreatedAt,
        UpdatedAt,
        ProgramsCount
    }
}
