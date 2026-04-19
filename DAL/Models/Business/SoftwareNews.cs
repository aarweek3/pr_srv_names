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
    [Table("software_news")]
    public class SoftwareNews
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareNewsConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(SoftwareNewsConstants.ShortDescriptionMaxLength)]
        public string? ShortDescription { get; set; }

        [MaxLength(SoftwareNewsConstants.ContentMaxLength)]
        public string? Content { get; set; }

        [MaxLength(SoftwareNewsConstants.ExternalUrlMaxLength)]
        public string? ExternalUrl { get; set; }

        public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public bool IsImportant { get; set; } = false;
        public bool IsPublished { get; set; } = true;
    }

}
