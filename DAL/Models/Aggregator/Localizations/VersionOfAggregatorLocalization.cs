using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("version_of_aggregator_localizations")]
    public class VersionOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? VersionOfAggregatorId { get; set; }
        [ForeignKey(nameof(VersionOfAggregatorId))]
        public virtual VersionOfAggregator? VersionOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        public string? WhatsNew { get; set; }
        public string? Description { get; set; }
    }
}
