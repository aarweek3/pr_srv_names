using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;

namespace DAL.Models.Aggregator
{
    /// <summary>
    /// Модель языка для агрегатора.
    /// Содержит информацию о локализации, кодах языка и региональных настройках.
    /// </summary>
    [Table("languages_of_aggregator")]
    public class LanguageOfAggregator : AuditableEntityOfAggregator
    {
        /// <summary>
        /// Полный код локализации (например, "ru-RU", "en-US").
        /// Используется для точной идентификации региональных настроек.
        /// </summary>
        [Required, MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Краткий код языка (например, "ru", "en").
        /// Часто используется в URL или как базовый идентификатор.
        /// </summary>
        [Required, MaxLength(10)]
        public string ShortCode { get; set; } = string.Empty;

        /// <summary>
        /// Название языка на английском или международное название (например, "Russian").
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Само название языка на этом же языке (например, "Русский").
        /// </summary>
        [Required, MaxLength(100)]
        public string NativeTitle { get; set; } = string.Empty;

        /// <summary>
        /// Флаг активности языка в системе.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Флаг основного языка системы (по умолчанию).
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Флаг письма справа налево (Right-to-Left).
        /// </summary>
        public bool IsRtl { get; set; } = false;

        /// <summary>
        /// Порядок сортировки в списках и меню.
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Флаг системного языка. Такие языки могут быть защищены от удаления.
        /// </summary>
        public bool IsSystem { get; set; } = false;

        /// <summary>
        /// Ключ иконки/флага (например, 'flag_ru').
        /// Используется для динамической подгрузки SVG через IconsController.
        /// </summary>
        [MaxLength(20)]
        public string? IconKey { get; set; }
    }
}
