using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    [Table("program_market_data_of_aggregator")]
    [Index(nameof(ProgramOfAggregatorId), nameof(LanguageOfAggregatorId), nameof(AggregatorSourceId), IsUnique = true)]
    public class ProgramMarketDataOfAggregator : FullAuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator LanguageOfAggregator { get; set; } = null!;

        public int AggregatorSourceId { get; set; }
        [ForeignKey(nameof(AggregatorSourceId))]
        public virtual AggregatorSource AggregatorSource { get; set; } = null!;

        /// <summary>
        /// Внешний идентификатор программы в магазине (например, AppID в Steam или PackageName в Google Play).
        /// </summary>
        [MaxLength(255)]
        public string? ExternalIdentifier { get; set; }

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
