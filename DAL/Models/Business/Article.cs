using DAL.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("article")]
    public class Article
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ArticleConstants.SlugMaxLength)]
        public string Slug { get; set; } = null!;

        public Guid? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        public Guid? AuthorId { get; set; }

        [MaxLength(ArticleConstants.ArticleTypeMaxLength)]
        public string? ArticleType { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? PublishedAt { get; set; }

        public bool IsPublished { get; set; } = false;
        public bool IsFeatured { get; set; } = false;

        public int ViewsCount { get; set; } = 0;

        public ICollection<ArticleTranslation> Translations { get; set; } = new List<ArticleTranslation>();
        public ICollection<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public ICollection<ArticleSoftware> ArticleSoftwares { get; set; } = new List<ArticleSoftware>();
        public ICollection<SeoEntity> SeoEntities { get; set; } = new List<SeoEntity>();
    }

}
