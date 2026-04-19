using DAL.Models.GeneralModels;
using DAL.Models.LocalizationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.SampleModels
{
    /// <summary>
    /// Таблица переводов для SampleMainSeo с поддержкой универсального SEO
    /// </summary>
    public class SampleMainDescriptionSeo
    {
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на родительскую сущность
        /// </summary>
        [Required]
        public int SampleMainSeoId { get; set; }
        public virtual SampleMainSeo SampleMainSeo { get; set; } = null!;

        /// <summary>
        /// Ссылка на язык перевода
        /// </summary>
        [Required]
        public int LanguageAppId { get; set; }
        public virtual LanguageApp LanguageApp { get; set; } = null!;

        /// <summary>
        /// Локализованное название
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Локализованное описание
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// HTML контент (статья/полное описание)
        /// </summary>
        public string? HtmlContent { get; set; }

        /// <summary>
        /// Локализованное изображение (если пусто, берется Main)
        /// </summary>
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        /// <summary>
        /// Связь с универсальной таблицей SEO (Композиция вместо наследования)
        /// </summary>
        public int? SeoDataId { get; set; }
        
        [ForeignKey("SeoDataId")]
        public virtual SeoData? SeoData { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
