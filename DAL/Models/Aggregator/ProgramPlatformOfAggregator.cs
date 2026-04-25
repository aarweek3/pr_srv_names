using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    // платформы на которых может работать программа. например windows 10/11
    [Table("program_platforms_of_aggregator")]
    [Index(nameof(ProgramOfAggregatorId), nameof(PlatformOfAggregatorId), IsUnique = true)]
    public class ProgramPlatformOfAggregator : AuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        public int PlatformOfAggregatorId { get; set; }
        [ForeignKey(nameof(PlatformOfAggregatorId))]
        public virtual PlatformOfAggregator PlatformOfAggregator { get; set; } = null!;

        [MaxLength(100)]
        public string? MinOsVersion { get; set; }
    }
}
