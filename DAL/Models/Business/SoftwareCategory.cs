using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("software_category")]
    public class SoftwareCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public bool IsPrimary { get; set; } = false;

        public short SortOrder { get; set; } = 0;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
