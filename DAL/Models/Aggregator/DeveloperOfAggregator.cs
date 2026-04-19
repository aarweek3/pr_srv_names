using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("developer_of_aggregators")]
    public class DeveloperOfAggregator : FullAuditableEntityOfAggregator
    {
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public string? Website { get; set; }
        public string? IconPath { get; set; }

        public virtual ICollection<DeveloperOfAggregatorLocalization> Localizations { get; set; }
            = new List<DeveloperOfAggregatorLocalization>();

        public virtual ICollection<ProgramOfAggregator> Programs { get; set; }
            = new List<ProgramOfAggregator>();
    }
}
