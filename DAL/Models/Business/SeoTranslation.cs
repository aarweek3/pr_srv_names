using DAL.Constants;
using DAL.Models.LocalizationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("seo_translation")]
    public class SeoTranslation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SeoEntityId { get; set; }

        [ForeignKey(nameof(SeoEntityId))]
        public SeoEntity SeoEntity { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(SeoTranslationConstants.SlugMaxLength)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(SeoTranslationConstants.MetaTitleMaxLength)]
        public string? MetaTitle { get; set; }

        [MaxLength(SeoTranslationConstants.MetaDescriptionMaxLength)]
        public string? MetaDescription { get; set; }

        [MaxLength(SeoTranslationConstants.KeywordsMaxLength)]
        public string? Keywords { get; set; }

        [MaxLength(SeoTranslationConstants.CanonicalUrlMaxLength)]
        public string? CanonicalUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
