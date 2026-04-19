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
    [Table("software_version")]
    public class SoftwareVersion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareVersionConstants.VersionNumberMaxLength)]
        public string VersionNumber { get; set; } = null!;

        public DateTimeOffset? ReleaseDate { get; set; }

        public bool IsLatest { get; set; } = false;
        public bool IsStable { get; set; } = true;
        public bool IsPreRelease { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<SoftwareVersionPlatform> VersionPlatforms { get; set; } = new List<SoftwareVersionPlatform>();
        public ICollection<UpdateFeed> UpdateFeeds { get; set; } = new List<UpdateFeed>();
    }

}
