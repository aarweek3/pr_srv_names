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
    [Table("tagBusiness")]

    public class TagBusiness
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TagConstants.SlugMaxLength)]
        public string Slug { get; set; } = null!;

        public int UsageCount { get; set; } = 0;

        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<SoftwareTag> SoftwareTags { get; set; } = new List<SoftwareTag>();
        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public ICollection<TagTranslation> Translations { get; set; } = new List<TagTranslation>();
    }

}
