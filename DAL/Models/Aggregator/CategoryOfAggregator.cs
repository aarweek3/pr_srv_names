using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Иерархическая категория программного обеспечения агрегатора.
    /// Позволяет группировать приложения по тематическим разделам (например, "Игры", "Офис").
    /// </summary>
    [Table("categories_of_aggregator")]
    [Index(nameof(Slug), IsUnique = true)]
    [Index(nameof(HierarchyPath))]
    public class CategoryOfAggregator : FullAuditableEntityOfAggregator
    {
        /// <summary>
        /// Уникальный технический идентификатор категории (например, "GAMES").
        /// Используется для программной идентификации в коде.
        /// </summary>
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        /// <summary>
        /// ЧПУ-идентификатор (URL-friendly идентификатор) категории.
        /// </summary>
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>Уровень вложенности (0 - корень).</summary>
        public int Level { get; set; } = 0;

        /// <summary>Материализованный путь (напр. "1/5/12/") для быстрых выборок всего дерева.</summary>
        [MaxLength(500)]
        public string HierarchyPath { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор родительской категории (для построения дерева).
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Родительская категория.
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual CategoryOfAggregator? Parent { get; set; }

        /// <summary>
        /// Дочерние категории.
        /// </summary>
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<CategoryOfAggregator> Children { get; set; }
            = new List<CategoryOfAggregator>();

        /// <summary>
        /// Путь к иконке категории (SVG/PNG) или код иконки.
        /// </summary>
        [MaxLength(255)]
        public string? IconPath { get; set; }

        /// <summary>
        /// Порядок сортировки в списках и меню.
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Флаг активности категории. Неактивные категории скрыты на фронтенде.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Флаг системной категории. Защищает важные категории от удаления.
        /// </summary>
        public bool IsSystem { get; set; } = false;

        /// <summary>
        /// Коллекция локализаций (переводов) категории.
        /// </summary>
        public virtual ICollection<CategoryOfAggregatorLocalization> Localizations { get; set; }
            = new List<CategoryOfAggregatorLocalization>();

        #region NotMapped Helpers

        /// <summary>
        /// Словарь локализованных названий (КодЯзыка -> Название).
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> LocalizedNames
        {
            get
            {
                if (Localizations == null || !Localizations.Any())
                    return new Dictionary<string, string>();

                return Localizations
                    .Where(loc => loc.LanguageOfAggregator != null)
                    .GroupBy(loc => loc.LanguageOfAggregator!.Code)
                    .ToDictionary(g => g.Key, g => g.First().Name);
            }
        }

        /// <summary>
        /// Словарь локализованных описаний (КодЯзыка -> Описание).
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> LocalizedDescriptions
        {
            get
            {
                if (Localizations == null || !Localizations.Any())
                    return new Dictionary<string, string>();

                return Localizations
                    .Where(loc => loc.LanguageOfAggregator != null && !string.IsNullOrEmpty(loc.Description))
                    .GroupBy(loc => loc.LanguageOfAggregator!.Code)
                    .ToDictionary(g => g.Key, g => g.First().Description!);
            }
        }

        /// <summary>
        /// Словарь локализованных мета-описаний SEO (КодЯзыка -> MetaDescription).
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> LocalizedMetaDescriptions
        {
            get
            {
                if (Localizations == null || !Localizations.Any())
                    return new Dictionary<string, string>();

                return Localizations
                    .Where(loc => loc.LanguageOfAggregator != null && !string.IsNullOrEmpty(loc.MetaDescription))
                    .GroupBy(loc => loc.LanguageOfAggregator!.Code)
                    .ToDictionary(g => g.Key, g => g.First().MetaDescription!);
            }
        }

        #endregion
    }
}

