using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Категория (группа) тегов агрегатора.
    /// Например: "Операционные системы", "Тип лицензии".
    /// </summary>
    [Table("category_tags_of_aggregator")]
    [Index(nameof(Slug), IsUnique = true)]
    public class CategoryTagOfAggregator : FullAuditableEntityOfAggregator
    {
        /// <summary>
        /// Технический идентификатор (для URL).
        /// </summary>
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Путь к иконке (SVG) для всей группы.
        /// </summary>
        [MaxLength(255)]
        public string? IconPath { get; set; }

        /// <summary>
        /// Цвет группы (HEX-код). Наследуется тегами, если у них не задан свой.
        /// </summary>
        [MaxLength(50)]
        public string? Color { get; set; }

        /// <summary>
        /// Порядок сортировки группы.
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Флаг активности группы.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Коллекция локализаций категории.
        /// </summary>
        public virtual ICollection<CategoryTagOfAggregatorLocalization> Localizations { get; set; }
            = new List<CategoryTagOfAggregatorLocalization>();

        /// <summary>
        /// Теги, принадлежащие данной категории.
        /// </summary>
        public virtual ICollection<TagOfAggregator> Tags { get; set; }
            = new List<TagOfAggregator>();
    }
}
