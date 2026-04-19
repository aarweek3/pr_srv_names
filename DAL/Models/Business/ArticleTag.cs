using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("article_tag")]
    public class ArticleTag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public Article Article { get; set; } = null!;

        [Required]
        public Guid TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public TagBusiness TagBusiness { get; set; } = null!;

        public short SortOrder { get; set; } = 0;

        public bool IsPrimary { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }


}
