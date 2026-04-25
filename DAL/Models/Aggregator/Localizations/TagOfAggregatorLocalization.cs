using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    /// <summary>
    /// Локализация тега агрегатора.
    /// Хранит название и описание тега на конкретном языке.
    /// </summary>
    [Table("tag_of_aggregator_localizations")]
    [Index(nameof(TagOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class TagOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        /// <summary>
        /// ID родительского тега.
        /// </summary>
        public int TagOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство к родительскому тегу.
        /// </summary>
        [ForeignKey(nameof(TagOfAggregatorId))]
        public virtual TagOfAggregator TagOfAggregator { get; set; } = null!;

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
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// SEO — тег <title>.
        /// </summary>
        [MaxLength(255)]
        public string? MetaTitle { get; set; }

        /// <summary>
        /// SEO — мета-описание.
        /// </summary>
        [MaxLength(500)]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// SEO — заголовок страницы тега.
        /// </summary>
        [MaxLength(255)]
        public string? H1Title { get; set; }
    }
}
