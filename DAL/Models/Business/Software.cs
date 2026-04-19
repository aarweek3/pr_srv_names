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
    [Table("software")]
    public class Software
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column("developer_id")]
        public Guid DeveloperId { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareConstants.SlugMaxLength)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public Guid LicenseTypeId { get; set; }

        [ForeignKey(nameof(LicenseTypeId))]
        public LicenseType LicenseType { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        [MaxLength(SoftwareConstants.ImportSourceMaxLength)]
        public string? ImportSource { get; set; }

        [MaxLength(SoftwareConstants.ImportIdMaxLength)]
        public string? ImportId { get; set; }

        // Навигационные свойства
        public ICollection<SoftwareTranslation> Translations { get; set; } = new List<SoftwareTranslation>();
        public ICollection<SoftwareVersion> Versions { get; set; } = new List<SoftwareVersion>();
        public ICollection<SoftwareCategory> SoftwareCategories { get; set; } = new List<SoftwareCategory>();
        public ICollection<SoftwareTag> SoftwareTags { get; set; } = new List<SoftwareTag>();
        public ICollection<SoftwareAsset> Assets { get; set; } = new List<SoftwareAsset>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<SoftwareLink> Links { get; set; } = new List<SoftwareLink>();
        public ICollection<SoftwareAlias> Aliases { get; set; } = new List<SoftwareAlias>();
        public ICollection<SoftwareNews> News { get; set; } = new List<SoftwareNews>();
        public ICollection<ArticleSoftware> ArticleSoftwares { get; set; } = new List<ArticleSoftware>();
        public SoftwareStatistics? Statistics { get; set; }
    }

}
