using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("download_link_of_aggregator_localizations")]
    public class DownloadLinkOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? DownloadLinkOfAggregatorId { get; set; }
        [ForeignKey(nameof(DownloadLinkOfAggregatorId))]
        public virtual DownloadLinkOfAggregator? DownloadLinkOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;
    }
}
