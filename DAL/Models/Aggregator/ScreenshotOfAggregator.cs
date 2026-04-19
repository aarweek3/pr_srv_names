using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("screenshots_of_aggregator")]
    public class ScreenshotOfAggregator : FullAuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator? ProgramOfAggregator { get; set; }

        [Required, MaxLength(2048)]
        public string FilePath { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;

        public virtual ICollection<ScreenshotOfAggregatorLocalization> Localizations { get; set; }
            = new List<ScreenshotOfAggregatorLocalization>();
    }
}
