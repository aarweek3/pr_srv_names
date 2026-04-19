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
    [Table("update_feed")]
    public class UpdateFeed
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareVersionId { get; set; }

        [ForeignKey(nameof(SoftwareVersionId))]
        public SoftwareVersion SoftwareVersion { get; set; } = null!;

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public LanguageApp Language { get; set; } = null!;

        [MaxLength(UpdateFeedConstants.TitleMaxLength)]
        public string? Title { get; set; }

        [MaxLength(UpdateFeedConstants.ChangesMaxLength)]
        public string? Changes { get; set; }

        public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.UtcNow;

        public bool IsMajor { get; set; } = false;

        public int SortOrder { get; set; } = 0;
    }

}
