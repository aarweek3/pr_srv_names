using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    /// <summary>
    /// Локализованные данные категории агрегатора.
    /// Хранит переводы названия, описания и SEO-метаданных.
    /// </summary>
    [Table("category_of_aggregator_localizations")]
    [Index(nameof(CategoryOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class CategoryOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int CategoryOfAggregatorId { get; set; }
        [ForeignKey(nameof(CategoryOfAggregatorId))]
        public virtual CategoryOfAggregator CategoryOfAggregator { get; set; } = null!;

        /// <summary>
        /// Идентификатор языка.
        /// </summary>
        public int LanguageOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство языка.
        /// </summary>
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        /// <summary>
        /// Локализованное название категории.
        /// </summary>
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Расширенное описание категории (может содержать HTML/Markdown).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// SEO заголовок страницы (Meta Title).
        /// </summary>
        [MaxLength(200)]
        public string? MetaTitle { get; set; }

        /// <summary>
        /// SEO описание страницы (Meta Description).
        /// </summary>
        [MaxLength(500)]
        public string? MetaDescription { get; set; }
    }
}

