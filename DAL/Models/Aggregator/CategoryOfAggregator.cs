using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    [Table("categories_of_aggregator")]
    public class CategoryOfAggregator : FullAuditableEntityOfAggregator
    {
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual CategoryOfAggregator? Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        public virtual ICollection<CategoryOfAggregator> Children { get; set; }
            = new List<CategoryOfAggregator>();

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<CategoryOfAggregatorLocalization> Localizations { get; set; }
            = new List<CategoryOfAggregatorLocalization>();
    }
}
