using System.ComponentModel.DataAnnotations;

namespace pr_srv_names.Pages.LanguageApp.Dtos
{
    /// <summary>
    /// Основной DTO для представления языка приложения.
    /// </summary>
    public class LanguageAppDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Код языка по стандарту BCP-47 (например, 'ru-RU')
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Краткий код языка (например, 'RU')
        /// </summary>
        public string ShortCode { get; set; } = null!;

        /// <summary>
        /// Название на английском
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Название на родном языке
        /// </summary>
        public string NativeTitle { get; set; } = null!;

        /// <summary>
        /// Доступность языка
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Направление письма ('ltr' или 'rtl')
        /// </summary>
        public string Direction { get; set; } = "ltr";

        /// <summary>
        /// Порядок сортировки (меньше = выше)
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Язык по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Системный язык
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Ключ иконки/флага
        /// </summary>
        public string? IconKey { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO для создания нового языка.
    /// </summary>
    public class CreateLanguageAppDto
    {
        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(5)]
        public string ShortCode { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NativeTitle { get; set; } = null!;

        public bool Enabled { get; set; } = true;

        [Required]
        [MaxLength(3)]
        public string Direction { get; set; } = "ltr";

        public int SortOrder { get; set; } = 999;

        public bool IsDefault { get; set; } = false;
        
        /// <summary>
        /// Системный язык (нельзя удалить)
        /// </summary>
        public bool IsSystem { get; set; } = false;

        [MaxLength(20)]
        public string? IconKey { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующего языка.
    /// </summary>
    public class UpdateLanguageAppDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(10)]
        public string? Code { get; set; }

        [MaxLength(5)]
        public string? ShortCode { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(50)]
        public string? NativeTitle { get; set; }

        public bool? Enabled { get; set; }

        [MaxLength(3)]
        public string? Direction { get; set; }

        public int? SortOrder { get; set; }

        public bool? IsDefault { get; set; }

        /// <summary>
        /// Системный язык
        /// </summary>
        public bool? IsSystem { get; set; }

        [MaxLength(20)]
        public string? IconKey { get; set; }
    }
}
