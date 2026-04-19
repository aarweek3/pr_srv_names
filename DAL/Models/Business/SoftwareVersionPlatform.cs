using DAL.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("software_version_platform")]
    public class SoftwareVersionPlatform
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareVersionId { get; set; }

        [ForeignKey(nameof(SoftwareVersionId))]
        public SoftwareVersion Version { get; set; } = null!;

        [Required]
        public Guid PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; } = null!;

        [MaxLength(SoftwareVersionPlatformConstants.ArchitectureMaxLength)]
        public string? Architecture { get; set; }

        public DateTimeOffset? ReleaseDate { get; set; }

        [MaxLength(SoftwareVersionPlatformConstants.SupportStatusMaxLength)]
        public string? SupportStatus { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<SoftwareVersionPlatformFile> Files { get; set; } = new List<SoftwareVersionPlatformFile>();
    }

}
