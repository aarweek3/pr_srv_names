using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Aggregator.Enums;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos
{
    public class VersionOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }

        public string? Changelog { get; set; }
        public string? Description { get; set; }
    }

    public class DownloadLinkOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        
        [Required, MaxLength(255)]
        public string Label { get; set; } = string.Empty;
    }

    public class DownloadLinkOfAggregatorDto
    {
        public int Id { get; set; }
        [Required, MaxLength(2048)]
        public string Url { get; set; } = string.Empty;
        public int? SourceId { get; set; }
        public bool IsExternal { get; set; }
        public int SortOrder { get; set; }
        public List<DownloadLinkOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    public class VersionOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string VersionNumber { get; set; } = string.Empty;
        public DateTimeOffset? ReleasedAt { get; set; }
        public bool IsLatest { get; set; }
        public VersionStatus Status { get; set; }
        public int DownloadLinksCount { get; set; }
    }

    public class VersionOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public int ProgramOfAggregatorId { get; set; }
        public string VersionNumber { get; set; } = string.Empty;
        public DateTimeOffset? ReleasedAt { get; set; }
        public bool IsLatest { get; set; }
        public int SortOrder { get; set; }
        public string? ExternalChangelogUrl { get; set; }
        public VersionStatus Status { get; set; }
        public List<VersionOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public List<DownloadLinkOfAggregatorDto> DownloadLinks { get; set; } = new();
    }

    public class VersionOfAggregatorCreateDto
    {
        public int ProgramOfAggregatorId { get; set; }
        
        [Required, MaxLength(50)]
        public string VersionNumber { get; set; } = string.Empty;
        
        public DateTimeOffset? ReleasedAt { get; set; }
        public bool IsLatest { get; set; }
        public int SortOrder { get; set; }
        public string? ExternalChangelogUrl { get; set; }
        public VersionStatus Status { get; set; } = VersionStatus.Stable;

        public List<VersionOfAggregatorLocalizationDto> Localizations { get; set; } = new();
        public List<DownloadLinkOfAggregatorDto> DownloadLinks { get; set; } = new();
    }

    public class VersionOfAggregatorUpdateDto : VersionOfAggregatorCreateDto
    {
        public int Id { get; set; }
    }
}
