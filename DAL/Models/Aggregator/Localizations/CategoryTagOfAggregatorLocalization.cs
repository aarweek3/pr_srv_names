using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    /// <summary>
    /// Локализация категории тегов агрегатора.
    /// </summary>
    [Table("category_tag_of_aggregator_localizations")]
    [Index(nameof(CategoryTagOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class CategoryTagOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int CategoryTagOfAggregatorId { get; set; }
        [ForeignKey(nameof(CategoryTagOfAggregatorId))]
        public virtual CategoryTagOfAggregator CategoryTag { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
