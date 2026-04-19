using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("category_of_aggregator_localizations")]
    public class CategoryOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? CategoryOfAggregatorId { get; set; }
        [ForeignKey(nameof(CategoryOfAggregatorId))]
        public virtual CategoryOfAggregator? CategoryOfAggregator { get; set; }

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
