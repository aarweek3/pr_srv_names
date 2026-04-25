using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Aggregator.Enums;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos
{
    public class ProgramOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public string? LicenseType { get; set; }
        
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public class ProgramOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string? DeveloperName { get; set; }
        public string? IconPath { get; set; }
        public ProgramStatus Status { get; set; }
        public long? TotalDownloads { get; set; }
        public double? AverageRating { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? LocalizedName { get; set; }
        public int VersionsCount { get; set; }
    }

    public class ProgramOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int CategoryOfAggregatorId { get; set; }
        public int? SubCategoryOfAggregatorId { get; set; }
        public int? DeveloperOfAggregatorId { get; set; }
        public string? IconPath { get; set; }
        public int SortOrder { get; set; }
        public bool NeedsReview { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public ProgramStatus Status { get; set; }
        public long? TotalDownloads { get; set; }
        public double? AverageRating { get; set; }
        public List<ProgramOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public List<int> PlatformIds { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class ProgramOfAggregatorCreateDto
    {
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public int CategoryOfAggregatorId { get; set; }
        public int? SubCategoryOfAggregatorId { get; set; }
        public int? DeveloperOfAggregatorId { get; set; }
        public string? IconPath { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool NeedsReview { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsSystem { get; set; } = false;
        public ProgramStatus Status { get; set; } = ProgramStatus.Draft;

        public List<ProgramOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public List<int> PlatformIds { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }

    public class ProgramOfAggregatorUpdateDto : ProgramOfAggregatorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class ProgramOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }
        public int? CategoryId { get; set; }
        public int? PlatformId { get; set; }
        public int? DeveloperId { get; set; }
        public ProgramStatus? Status { get; set; }
        public ProgramOfAggregatorSortField SortBy { get; set; } = ProgramOfAggregatorSortField.CreatedAt;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Desc;
        public bool ShowDeleted { get; set; }
    }

    public class ProgramOfAggregatorPagedResponseDto
    {
        public IEnumerable<ProgramOfAggregatorItemDto> Items { get; set; } = new List<ProgramOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum ProgramOfAggregatorSortField
    {
        Id,
        CanonicalName,
        Slug,
        SortOrder,
        CreatedAt,
        TotalDownloads,
        AverageRating
    }
}
