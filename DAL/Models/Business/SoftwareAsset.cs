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
    [Table("software_asset")]
    public class SoftwareAsset
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        public Guid? PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public Platform? Platform { get; set; }

        [Required]
        [MaxLength(SoftwareAssetConstants.AssetTypeMaxLength)]
        public string AssetType { get; set; } = string.Empty;

        [Required]
        [MaxLength(SoftwareAssetConstants.UrlMaxLength)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(SoftwareAssetConstants.TitleMaxLength)]
        public string? Title { get; set; }

        [MaxLength(SoftwareAssetConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public bool IsPrimary { get; set; } = false;

        public short SortOrder { get; set; } = 0;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
