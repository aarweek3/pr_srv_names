using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("screenshot_of_aggregator_localizations")]
    [Index(nameof(ScreenshotOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class ScreenshotOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int ScreenshotOfAggregatorId { get; set; }
        [ForeignKey(nameof(ScreenshotOfAggregatorId))]
        public virtual ScreenshotOfAggregator ScreenshotOfAggregator { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [MaxLength(500)]
        public string? AltText { get; set; }
    }
}
