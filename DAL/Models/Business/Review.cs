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
    [Table("review")]
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        [Range(ReviewConstants.MinScore, ReviewConstants.MaxScore)]
        public short Score { get; set; }

        [MaxLength(ReviewConstants.ContentMaxLength)]
        public string? Content { get; set; }

        public bool IsRatingOnly { get; set; } = false;

        public bool IsApproved { get; set; } = false;
        public bool IsEdited { get; set; } = false;

        public int HelpfulCount { get; set; } = 0;
        public int UnhelpfulCount { get; set; } = 0;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
