using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Enums;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("versions_of_aggregator")]
    [Index(nameof(ProgramOfAggregatorId), nameof(VersionNumber), IsUnique = true)]
    public class VersionOfAggregator : FullAuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        [Required, MaxLength(50)]
        public string VersionNumber { get; set; } = string.Empty;

        public DateTimeOffset? ReleasedAt { get; set; }

        public bool IsLatest { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        [MaxLength(2048)]
        public string? ExternalChangelogUrl { get; set; }

        public VersionStatus Status { get; set; } = VersionStatus.Stable;

        public virtual ICollection<VersionOfAggregatorLocalization> Localizations { get; set; }
            = new List<VersionOfAggregatorLocalization>();

        public virtual ICollection<DownloadLinkOfAggregator> DownloadLinks { get; set; }
            = new List<DownloadLinkOfAggregator>();
    }
}
