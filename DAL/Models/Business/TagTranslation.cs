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
    [Table("tag_translation")]
    public class TagTranslation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public TagBusiness TagBusiness { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(TagTranslationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(TagTranslationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
