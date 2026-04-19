using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Enums;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("download_links_of_aggregator")]
    public class DownloadLinkOfAggregator : FullAuditableEntityOfAggregator
    {
        public int VersionOfAggregatorId { get; set; }
        [ForeignKey(nameof(VersionOfAggregatorId))]
        public virtual VersionOfAggregator? VersionOfAggregator { get; set; }

        [Required, MaxLength(2048)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Architecture { get; set; }

        public long? FileSize { get; set; }
        [MaxLength(128)]
        public string? FileHash { get; set; }

        public DownloadStatus Status { get; set; } = DownloadStatus.Unknown;

        public virtual ICollection<DownloadLinkOfAggregatorLocalization> Localizations { get; set; }
            = new List<DownloadLinkOfAggregatorLocalization>();
    }
}
