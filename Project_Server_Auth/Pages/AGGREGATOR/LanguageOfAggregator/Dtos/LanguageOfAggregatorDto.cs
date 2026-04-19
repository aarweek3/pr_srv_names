using System;
using System.ComponentModel.DataAnnotations;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Constants;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos
{
    /// <summary>
    /// Основной DTO для представления языка агрегатора.
    /// </summary>
    public class LanguageOfAggregatorDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Полный код локализации (например, "ru-RU").
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Краткий код языка (например, "ru").
        /// </summary>
        public string ShortCode { get; set; } = null!;

        /// <summary>
        /// Название на английском.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Название на родном языке.
        /// </summary>
        public string NativeTitle { get; set; } = null!;

        /// <summary>
        /// Доступность языка в системе.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Язык по умолчанию.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Письмо справа налево.
        /// </summary>
        public bool IsRtl { get; set; }

        /// <summary>
        /// Системный язык (защищен от удаления).
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Ключ иконки/флага.
        /// </summary>
        public string? IconKey { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO для создания нового языка агрегатора.
    /// </summary>
    public class CreateLanguageOfAggregatorDto
    {
        [Required]
        [MaxLength(LanguageOfAggregatorValidationConstants.CodeMaxLength)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(LanguageOfAggregatorValidationConstants.ShortCodeMaxLength)]
        public string ShortCode { get; set; } = null!;

        [Required]
        [MaxLength(LanguageOfAggregatorValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(LanguageOfAggregatorValidationConstants.NativeTitleMaxLength)]
        public string NativeTitle { get; set; } = null!;

        public bool Enabled { get; set; } = true;
        public bool IsDefault { get; set; } = false;
        public bool IsRtl { get; set; } = false;
        public bool IsSystem { get; set; } = false;
        public int SortOrder { get; set; } = 0;

        [MaxLength(20)]
        public string? IconKey { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующего языка агрегатора.
    /// </summary>
    public class UpdateLanguageOfAggregatorDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(LanguageOfAggregatorValidationConstants.CodeMaxLength)]
        public string? Code { get; set; }

        [MaxLength(LanguageOfAggregatorValidationConstants.ShortCodeMaxLength)]
        public string? ShortCode { get; set; }

        [MaxLength(LanguageOfAggregatorValidationConstants.TitleMaxLength)]
        public string? Title { get; set; }

        [MaxLength(LanguageOfAggregatorValidationConstants.NativeTitleMaxLength)]
        public string? NativeTitle { get; set; }

        public bool? Enabled { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsRtl { get; set; }
        public bool? IsSystem { get; set; }
        public int? SortOrder { get; set; }

        [MaxLength(20)]
        public string? IconKey { get; set; }
    }
}
