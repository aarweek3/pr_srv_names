using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Редиректы для Slug программ.
    /// Хранит старые URL-префиксы, чтобы перенаправлять пользователей на новые при переименовании.
    /// </summary>
    [Table("program_slug_redirects_of_aggregator")]
    [Index(nameof(OldSlug), IsUnique = true)]
    public class ProgramSlugRedirectOfAggregator : AuditableEntityOfAggregator
    {
        /// <summary>
        /// Старый Slug (префикс URL).
        /// </summary>
        [Required, MaxLength(100)]
        public string OldSlug { get; set; } = string.Empty;

        /// <summary>
        /// Новый (текущий) Slug программы.
        /// </summary>
        [Required, MaxLength(100)]
        public string NewSlug { get; set; } = string.Empty;

        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;
    }
}
