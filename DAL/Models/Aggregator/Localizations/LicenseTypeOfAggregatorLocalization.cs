using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("license_type_of_aggregator_localizations")]
    public class LicenseTypeOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? LicenseTypeOfAggregatorId { get; set; }
        [ForeignKey(nameof(LicenseTypeOfAggregatorId))]
        public virtual LicenseTypeOfAggregator? LicenseTypeOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
