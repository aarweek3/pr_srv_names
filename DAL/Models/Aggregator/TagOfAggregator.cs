using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;
using DAL.Models.Aggregator.Enums;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Сущность тега (метки) для программ агрегатора.
    /// Позволяет группировать программы по произвольным признакам.
    /// </summary>
    [Table("tags_of_aggregator")]
    [Index(nameof(Slug), IsUnique = true)]
    public class TagOfAggregator : FullAuditableEntityOfAggregator
    {
        /// <summary>
        /// Уникальный слаг (URL-friendly идентификатор) тега.
        /// </summary>
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор категории (группы) тега.
        /// </summary>
        public int CategoryTagId { get; set; }

        /// <summary>
        /// Родительская категория тега.
        /// </summary>
        [ForeignKey(nameof(CategoryTagId))]
        public virtual CategoryTagOfAggregator Category { get; set; } = null!;

        /// <summary>
        /// Тип тега.
        /// </summary>
        public TagType Type { get; set; } = TagType.Functional;

        /// <summary>
        /// Цвет тега (HEX-код). Если "inherit", берется цвет категории.
        /// </summary>
        [Required, MaxLength(50)]
        public string Color { get; set; } = "inherit";

        /// <summary>
        /// Путь к кастомной иконке (SVG). Если null, берется иконка категории.
        /// </summary>
        [MaxLength(255)]
        public string? IconPath { get; set; }

        /// <summary>
        /// Флаг приоритетного вывода (важная характеристика).
        /// </summary>
        public bool IsFeature { get; set; } = false;

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
