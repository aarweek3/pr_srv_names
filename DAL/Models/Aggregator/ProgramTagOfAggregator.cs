using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Таблица связи программ с тегами (многие-ко-многим).
    /// </summary>
    [Table("program_tags_of_aggregator")]
    [Index(nameof(ProgramOfAggregatorId), nameof(TagOfAggregatorId), IsUnique = true)]
    public class ProgramTagOfAggregator : AuditableEntityOfAggregator
    {
        /// <summary>
        /// ID программы.
        /// </summary>
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        /// <summary>
        /// ID тега.
        /// </summary>
        public int TagOfAggregatorId { get; set; }
        [ForeignKey(nameof(TagOfAggregatorId))]
        public virtual TagOfAggregator TagOfAggregator { get; set; } = null!;

        /// <summary>
        /// Порядок сортировки тегов внутри программы.
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Флаг основного тега для программы.
        /// </summary>
        public bool IsMain { get; set; } = false;
    }
}
