using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    [Table("download_logs_of_aggregator")]
    [Index(nameof(VersionOfAggregatorId))]
    [Index(nameof(DownloadLinkOfAggregatorId))]
    [Index(nameof(CreatedAt))]
    public class DownloadLogOfAggregator : BaseEntityOfAggregator
    {
        public int? VersionOfAggregatorId { get; set; }
        [ForeignKey(nameof(VersionOfAggregatorId))]
        public virtual VersionOfAggregator? VersionOfAggregator { get; set; }

        public int? DownloadLinkOfAggregatorId { get; set; }
        [ForeignKey(nameof(DownloadLinkOfAggregatorId))]
        public virtual DownloadLinkOfAggregator? DownloadLinkOfAggregator { get; set; }


        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(1000)]
        public string? UserAgent { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
