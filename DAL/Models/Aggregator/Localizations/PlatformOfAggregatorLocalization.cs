using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.GeneralModels;

namespace DAL.Models.Aggregator.Localizations
{
    // Локализация (перевод) для платформы агрегатора
    [Table("platform_of_aggregator_localizations")]
    public class PlatformOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? PlatformOfAggregatorId { get; set; }
        [ForeignKey(nameof(PlatformOfAggregatorId))]
        public virtual PlatformOfAggregator? PlatformOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        // Локализованное название
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        // Краткое описание
        public string? Description { get; set; }

        // Полное описание (HTML)
        public string? HtmlContent { get; set; }

        // Локализованное изображение (иконка или скриншот для конкретного языка)
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        // Связь с SEO данными (Композиция)
        public int? SeoDataId { get; set; }

        [ForeignKey("SeoDataId")]
        public virtual SeoData? SeoData { get; set; }
    }
}
