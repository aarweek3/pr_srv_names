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
    [Table("software_version_platform_file")]
    public class SoftwareVersionPlatformFile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareVersionPlatformId { get; set; }

        [ForeignKey(nameof(SoftwareVersionPlatformId))]
        public SoftwareVersionPlatform VersionPlatform { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareVersionPlatformFileConstants.FileTypeMaxLength)]
        public string FileType { get; set; } = string.Empty;

        [MaxLength(SoftwareVersionPlatformFileConstants.FileFormatMaxLength)]
        public string? FileFormat { get; set; }

        [Required]
        [MaxLength(SoftwareVersionPlatformFileConstants.UrlMaxLength)]
        public string Url { get; set; } = string.Empty;

        public Guid? MirrorId { get; set; }

        [MaxLength(SoftwareVersionPlatformFileConstants.Sha256MaxLength)]
        public string? Sha256 { get; set; }

        public long? SizeBytes { get; set; }

        public bool IsPrimary { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
