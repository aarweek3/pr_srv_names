using DAL.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    // Платформа (ОС) для Агрегатора
    [Table("platforms_of_aggregator")]
    public class PlatformOfAggregator : FullAuditableEntityOfAggregator
    {
        // Публичное/системное название
        [Required, MaxLength(PlatformConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        // Системный код (slug) для URL
        [Required, MaxLength(PlatformConstants.CodeMaxLength)]
        public string SystemCode { get; set; } = string.Empty;

        // Путь к иконке
        public string? IconPath { get; set; }
        
        // Флаг активности
        public bool IsActive { get; set; } = true;
        
        // Порядок сортировки
        public int SortOrder { get; set; } = 0;

        public virtual ICollection<PlatformOfAggregatorLocalization> Localizations { get; set; }
            = new List<PlatformOfAggregatorLocalization>();

        public virtual ICollection<ProgramPlatformOfAggregator> ProgramPlatforms { get; set; }
            = new List<ProgramPlatformOfAggregator>();
    }
}
