using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("developer_of_aggregator_localizations")]
    public class DeveloperOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? DeveloperOfAggregatorId { get; set; }
        [ForeignKey(nameof(DeveloperOfAggregatorId))]
        public virtual DeveloperOfAggregator? DeveloperOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
