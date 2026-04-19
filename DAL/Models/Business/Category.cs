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
    [Table("category")]
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(CategoryConstants.SlugMaxLength)]
        public string Slug { get; set; } = null!;

        public Guid? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Category? Parent { get; set; }

        [MaxLength(CategoryConstants.PathMaxLength)]
        public string Path { get; set; } = string.Empty;

        public short Level { get; set; } = 0;

        public bool IsLeaf { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 1000;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Category> Children { get; set; } = new List<Category>();
        public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
        public ICollection<SoftwareCategory> SoftwareCategories { get; set; } = new List<SoftwareCategory>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<SeoEntity> SeoEntities { get; set; } = new List<SeoEntity>();
    }

}
