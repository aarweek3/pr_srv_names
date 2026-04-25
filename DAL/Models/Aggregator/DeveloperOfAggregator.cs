using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;
using DAL.Constants;

namespace DAL.Models.Aggregator
{
    [Table("developers_of_aggregator")]
    [Index(nameof(SystemCode), IsUnique = true)]
    public class DeveloperOfAggregator : FullAuditableEntityOfAggregator
    {
        [Required, MaxLength(DeveloperConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(DeveloperConstants.SlugMaxLength)]
        public string SystemCode { get; set; } = string.Empty;

        public string? Website { get; set; }
        public string? IconPath { get; set; }

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public virtual ICollection<DeveloperOfAggregatorLocalization> Localizations { get; set; }
            = new List<DeveloperOfAggregatorLocalization>();

        public virtual ICollection<ProgramOfAggregator> Programs { get; set; }
            = new List<ProgramOfAggregator>();
    }
}
