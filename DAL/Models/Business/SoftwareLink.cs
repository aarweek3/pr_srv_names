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
    [Table("software_link")]
    public class SoftwareLink
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareLinkConstants.UrlMaxLength)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [MaxLength(SoftwareLinkConstants.LinkTypeMaxLength)]
        public string LinkType { get; set; } = string.Empty;

        [MaxLength(SoftwareLinkConstants.TitleMaxLength)]
        public string? Title { get; set; }

        [MaxLength(SoftwareLinkConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public short SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
