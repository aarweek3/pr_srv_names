using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("videos_of_aggregator")]
    public class VideoOfAggregator : FullAuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        [Required, MaxLength(2048)]
        public string VideoUrl { get; set; } = string.Empty;

        public string? ThumbnailPath { get; set; }
        public int SortOrder { get; set; } = 0;

        public virtual ICollection<VideoOfAggregatorLocalization> Localizations { get; set; }
            = new List<VideoOfAggregatorLocalization>();
    }
}
