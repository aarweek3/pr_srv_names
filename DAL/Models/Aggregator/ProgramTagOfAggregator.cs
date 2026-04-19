using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Таблица связи программ с тегами (многие-ко-многим).
    /// </summary>
    [Table("program_tags_of_aggregator")]
    public class ProgramTagOfAggregator : AuditableEntityOfAggregator
    {
        /// <summary>
        /// ID программы.
        /// </summary>
        public int? ProgramOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство к программе.
        /// </summary>
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator? ProgramOfAggregator { get; set; }

        /// <summary>
        /// ID тега.
        /// </summary>
        public int? TagOfAggregatorId { get; set; }

        /// <summary>
        /// Навигационное свойство к тегу.
        /// </summary>
        [ForeignKey(nameof(TagOfAggregatorId))]
        public virtual TagOfAggregator? TagOfAggregator { get; set; }

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
