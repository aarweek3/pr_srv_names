using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("download_link_of_aggregator_localizations")]
    [Index(nameof(DownloadLinkOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class DownloadLinkOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int DownloadLinkOfAggregatorId { get; set; }
        [ForeignKey(nameof(DownloadLinkOfAggregatorId))]
        public virtual DownloadLinkOfAggregator DownloadLinkOfAggregator { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;
    }
}
