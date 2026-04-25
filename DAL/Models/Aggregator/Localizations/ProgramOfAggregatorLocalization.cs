using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator.Localizations
{
    [Table("programs_of_aggregator_localizations")]
    [Index(nameof(ProgramOfAggregatorId), nameof(LanguageOfAggregatorId), IsUnique = true)]
    public class ProgramOfAggregatorLocalization : AuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        public int LanguageOfAggregatorId { get; set; }
        [ForeignKey(nameof(LanguageOfAggregatorId))]
        public virtual LanguageOfAggregator LanguageOfAggregator { get; set; } = null!;

        public int? LicenseTypeId { get; set; }
        [ForeignKey(nameof(LicenseTypeId))]
        public virtual LicenseTypeOfAggregator? LicenseType { get; set; }

        [MaxLength(255)]
        public string? LocalizedName { get; set; }

        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
