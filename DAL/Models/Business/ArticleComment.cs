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
    [Table("article_comment")]
    public class ArticleComment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public Article Article { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        public Guid? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public ArticleComment? Parent { get; set; }

        [Required]
        [MaxLength(ArticleCommentConstants.ContentMaxLength)]
        public string Content { get; set; } = null!;

        public short Rating { get; set; } = 0;

        public bool IsApproved { get; set; } = false;
        public bool ContainsSpam { get; set; } = false;
        public bool IsEdited { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<ArticleComment> Replies { get; set; } = new List<ArticleComment>();
    }

}
