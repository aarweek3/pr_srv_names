using DAL.Constants;
using DAL.Models.LocalizationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("software_translation")]
    public class SoftwareTranslation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column("software_id")]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        [Column("language_id")]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareTranslationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(SoftwareTranslationConstants.ShortDescriptionMaxLength)]
        public string? ShortDescription { get; set; }

        [MaxLength(SoftwareTranslationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
