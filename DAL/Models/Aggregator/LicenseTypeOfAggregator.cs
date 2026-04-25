using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("license_types_of_aggregator")]
    [Index(nameof(Slug), IsUnique = true)]
    public class LicenseTypeOfAggregator : FullAuditableEntityOfAggregator
    {
        [Required, MaxLength(100)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public virtual ICollection<LicenseTypeOfAggregatorLocalization> Localizations { get; set; }
            = new List<LicenseTypeOfAggregatorLocalization>();
    }
}
