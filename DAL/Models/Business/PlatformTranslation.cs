using DAL.Constants;
using DAL.Models.GeneralModels;
using DAL.Models.LocalizationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Business
{
    /// <summary>
    /// Локализованные данные для платформы (название, описание, SEO)
    /// </summary>
    [Table("platform_translation")]
    public class PlatformTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public virtual Platform Platform { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual LanguageApp Language { get; set; } = null!;

        /// <summary>
        /// Локализованное название платформы (например, "Виндовс" для RU)
        /// </summary>
        [Required]
        [MaxLength(PlatformConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Локализованное описание платформы
        /// </summary>
        [MaxLength(PlatformConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Полное описание (HTML контент из редактора)
        /// </summary>
        public string? DescriptionFull { get; set; }

        /// <summary>
        /// Локализованное изображение (переопределяет UrlPictureMain)
        /// </summary>
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        /// <summary>
        /// Связь с универсальной таблицей SEO
        /// </summary>
        public int? SeoDataId { get; set; }

        [ForeignKey(nameof(SeoDataId))]
        public virtual SeoData? SeoData { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
