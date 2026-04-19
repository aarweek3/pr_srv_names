using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.GeneralModels
{
    /// <summary>
    /// Модель для хранения ВСЕХ SEO-данных. 
    /// Чистая сущность (POCO) для использования через композицию. 
    /// Вся логика вынесена в SeoDataExtensions.
    /// </summary>
    public class SeoData
    {
        [Key] public int Id { get; set; }

        // =================================================================
        // ОСНОВНЫЕ META ТЕГИ
        // =================================================================

        [MaxLength(70)]
        public string? MetaTitle { get; set; }

        [MaxLength(160)]
        public string? MetaDescription { get; set; }

        [MaxLength(200)]
        public string? MetaKeywords { get; set; }

        [MaxLength(200)]
        public string? UrlSlug { get; set; }

        [MaxLength(300)]
        public string? CanonicalUrl { get; set; }

        // =================================================================
        // OPEN GRAPH ТЕГИ
        // =================================================================

        [MaxLength(100)]
        public string? OgTitle { get; set; }

        [MaxLength(200)]
        public string? OgDescription { get; set; }

        [MaxLength(500)]
        public string? OgImage { get; set; }

        [MaxLength(50)]
        public string? OgType { get; set; } = "article";

        [MaxLength(300)]
        public string? OgUrl { get; set; }

        // =================================================================
        // TWITTER CARD ТЕГИ
        // =================================================================

        [MaxLength(100)]
        public string? TwitterCard { get; set; } = "summary_large_image";

        [MaxLength(100)]
        public string? TwitterTitle { get; set; }

        [MaxLength(200)]
        public string? TwitterDescription { get; set; }

        [MaxLength(500)]
        public string? TwitterImage { get; set; }

        // =================================================================
        // ИЗОБРАЖЕНИЯ И ALT ТЕГИ
        // =================================================================

        [MaxLength(500)] public string? ImageUrl { get; set; } = string.Empty;

        [MaxLength(125)]
        public string? ImageAltText { get; set; }

        [MaxLength(200)] public string? ImageCaption { get; set; }

        // =================================================================
        // STRUCTURED DATA (SCHEMA.ORG)
        // =================================================================

        [MaxLength(50)]
        public string? SchemaType { get; set; }

        [Column(TypeName = "jsonb")]
        public string? SchemaJsonLd { get; set; }

        // =================================================================
        // АВТОР И ПУБЛИКАЦИЯ
        // =================================================================

        [MaxLength(100)] public string? AuthorName { get; set; } = string.Empty;

        [MaxLength(100)] public string? PublisherName { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        [MaxLength(100)] public string? ArticleSection { get; set; } = string.Empty;

        // =================================================================
        // ДОПОЛНИТЕЛЬНЫЕ SEO ПОЛЯ
        // =================================================================

        public bool NoIndex { get; set; } = false;

        public bool NoFollow { get; set; } = false;

        [Range(0, 10)]
        public int? Priority { get; set; } = 5;

        [MaxLength(100)]
        public string? Region { get; set; }
    }
}