using System.ComponentModel.DataAnnotations;

namespace DAL.Models.LocalizationModels
{
    /// <summary>
    /// Сущность языка интерфейса приложения
    /// </summary>
    public class LanguageApp
    {
        /// <summary>
        /// Уникальный идентификатор языка
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код языка по стандарту BCP-47
        /// Примеры: 'ru-RU', 'en-US', 'de-DE'
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = null!;

        /// <summary>
        /// Краткий код языка для UI
        /// Примеры: 'RU', 'EN', 'DE'
        /// </summary>
        [Required]
        [MaxLength(5)]
        public string ShortCode { get; set; } = null!;

        /// <summary>
        /// Название на английском (для админки/логов)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Название на родном языке (для UI)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NativeTitle { get; set; } = null!;

        /// <summary>
        /// Доступность языка для выбора
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Направление письма
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string Direction { get; set; } = "ltr"; // 'ltr' или 'rtl'

        /// <summary>
        /// Порядок сортировки в UI (меньше = выше)
        /// </summary>
        public int SortOrder { get; set; } = 999;

        /// <summary>
        /// Язык по умолчанию (должен быть только один)
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Системный язык (нельзя удалить)
        /// </summary>
        public bool IsSystem { get; set; } = false;

        /// <summary>
        /// Ключ иконки/флага
        /// </summary>
        [MaxLength(20)]
        public string? IconKey { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

       
    }
}