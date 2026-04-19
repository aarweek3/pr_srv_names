using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    [Table("program_market_data_of_aggregator")]
    public class ProgramMarketDataOfAggregator : FullAuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator? ProgramOfAggregator { get; set; }

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator? LanguageOfAggregator { get; set; }

        public int AggregatorSourceId { get; set; }
        [ForeignKey(nameof(AggregatorSourceId))]
        public virtual AggregatorSource? AggregatorSource { get; set; }

        public double? RatingValue { get; set; }
        public double? RatingMax { get; set; }

        public double? RatingNormalized { get; set; }

        public long? DownloadCountExact { get; set; }

        [MaxLength(100)]
        public string? DownloadCountDisplay { get; set; }

        [MaxLength(100)]
        public string? Price { get; set; }
    }
}
