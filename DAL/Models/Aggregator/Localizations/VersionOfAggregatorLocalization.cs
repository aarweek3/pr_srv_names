using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("version_of_aggregator_localizations")]
    [Index(nameof(VersionOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class VersionOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int VersionOfAggregatorId { get; set; }
        [ForeignKey(nameof(VersionOfAggregatorId))]
        public virtual VersionOfAggregator VersionOfAggregator { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        public string? WhatsNew { get; set; }
        public string? Description { get; set; }
    }
}
