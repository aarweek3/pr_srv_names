using DAL.Models.Aggregator.Base;
using DAL.Models.GeneralModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.Aggregator.Localizations
{
    // Локализация (перевод) для типа лицензии агрегатора
    [Table("license_type_of_aggregator_localizations")]
    [Index(nameof(LicenseTypeOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class LicenseTypeOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int LicenseTypeOfAggregatorId { get; set; }
        [ForeignKey(nameof(LicenseTypeOfAggregatorId))]
        public virtual LicenseTypeOfAggregator LicenseTypeOfAggregator { get; set; } = null!;

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

        // Локализованное изображение (опционально)
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        // Связь с SEO данными
        public int? SeoDataId { get; set; }
        [ForeignKey(nameof(SeoDataId))]
        public virtual SeoData? SeoData { get; set; }
    }
}
