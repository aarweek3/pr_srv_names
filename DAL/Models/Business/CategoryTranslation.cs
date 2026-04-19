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
    [Table("category_translation")]
    public class CategoryTranslation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(CategoryTranslationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(CategoryTranslationConstants.ShortDescriptionMaxLength)]
        public string? ShortDescription { get; set; }

        [MaxLength(CategoryTranslationConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
