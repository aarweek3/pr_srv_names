using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Снимок состояния программы (Snapshot).
    /// Хранит исторические данные о цене, рейтинге и скачиваниях на конкретный момент времени.
    /// </summary>
    [Table("program_snapshots_of_aggregator")]
    [Index(nameof(ProgramOfAggregatorId), nameof(SnapshotDate))]
    public class ProgramSnapshotOfAggregator : AuditableEntityOfAggregator
    {
        public int ProgramOfAggregatorId { get; set; }
        [ForeignKey(nameof(ProgramOfAggregatorId))]
        public virtual ProgramOfAggregator ProgramOfAggregator { get; set; } = null!;

        public int AggregatorSourceId { get; set; }
        [ForeignKey(nameof(AggregatorSourceId))]
        public virtual AggregatorSource AggregatorSource { get; set; } = null!;

        /// <summary>Цена на момент снимка.</summary>
        [MaxLength(100)]
        public string? Price { get; set; }

        /// <summary>Рейтинг на момент снимка.</summary>
        public double? RatingValue { get; set; }

        /// <summary>Кол-во скачиваний на момент снимка.</summary>
        public long? DownloadCount { get; set; }

        /// <summary>Точная дата фиксации данных.</summary>
        public DateTime SnapshotDate { get; set; } = DateTime.UtcNow;
    }
}
