using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("video_of_aggregator_localizations")]
    public class VideoOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int? VideoOfAggregatorId { get; set; }
        [ForeignKey(nameof(VideoOfAggregatorId))]
        public virtual VideoOfAggregator? VideoOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
