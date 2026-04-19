using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("software_statistics")]
    public class SoftwareStatistics
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        public long ViewsCount { get; set; } = 0;
        public long DownloadCount { get; set; } = 0;

        public double AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public int RatingOnlyCount { get; set; } = 0;

        public int BookmarkCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;

        public DateTimeOffset LastCalculatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
