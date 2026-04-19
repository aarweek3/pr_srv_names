using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Сущность тега (метки) для программ агрегатора.
    /// Позволяет группировать программы по произвольным признакам.
    /// </summary>
    [Table("tags_of_aggregator")]
    public class TagOfAggregator : FullAuditableEntityOfAggregator
    {
        /// <summary>
        /// Уникальный слаг (URL-friendly идентификатор) тега.
        /// </summary>
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Порядок сортировки тега.
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Флаг активности тега в системе.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Коллекция локализаций тега.
        /// </summary>
        public virtual ICollection<TagOfAggregatorLocalization> Localizations { get; set; }
            = new List<TagOfAggregatorLocalization>();

        /// <summary>
        /// Связи программ с данным тегом.
        /// </summary>
        public virtual ICollection<ProgramTagOfAggregator> ProgramTags { get; set; }
            = new List<ProgramTagOfAggregator>();
    }
}
