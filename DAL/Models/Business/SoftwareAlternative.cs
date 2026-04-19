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
    [Table("software_alternative")]
    public class SoftwareAlternative
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FromSoftwareId { get; set; }

        [ForeignKey(nameof(FromSoftwareId))]
        public Software FromSoftware { get; set; } = null!;

        [Required]
        public Guid ToSoftwareId { get; set; }

        [ForeignKey(nameof(ToSoftwareId))]
        public Software ToSoftware { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareAlternativeConstants.RelationTypeMaxLength)]
        public string RelationType { get; set; } = string.Empty;

        public bool IsMutual { get; set; } = false;

        public short SortOrder { get; set; } = 0;

        [MaxLength(SoftwareAlternativeConstants.CommentMaxLength)]
        public string? Comment { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
