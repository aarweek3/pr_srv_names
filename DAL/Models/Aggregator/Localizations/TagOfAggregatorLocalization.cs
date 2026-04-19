using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    /// <summary>
    /// Локализация тега агрегатора.
    /// Хранит название и описание тега на конкретном языке.
    /// </summary>
    [Table("tag_of_aggregator_localizations")]
    public class TagOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        /// <summary>
        /// ID родительского тега.
        /// </summary>
        public int? TagOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство к родительскому тегу.
        /// </summary>
        [ForeignKey(nameof(TagOfAggregatorId))]
        public virtual TagOfAggregator? TagOfAggregator { get; set; }

        /// <summary>
        /// ID языка локализации.
        /// </summary>
        public int LanguageOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство к языку.
        /// </summary>
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        /// <summary>
        /// Локализованное название тега.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Локализованное описание тега.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
