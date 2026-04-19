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
    [Table("article_translation")]
    public class ArticleTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public Article Article { get; set; } = null!;

        [Required]
        [Column("language_id")]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(ArticleTranslationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ArticleTranslationConstants.ContentMaxLength)]
        public string Content { get; set; } = null!;

        [MaxLength(ArticleTranslationConstants.ShortSummaryMaxLength)]
        public string? ShortSummary { get; set; }

        [MaxLength(ArticleTranslationConstants.CoverImageUrlMaxLength)]
        public string? CoverImageUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
