using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Constants;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos
{
    /// <summary>
    /// Локализация для платформы агрегатора с поддержкой SEO
    /// </summary>
    public class PlatformOfAggregatorLocalizationDto
    {
        public int LanguageOfAggregatorId { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageNativeName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? HtmlContent { get; set; }
        
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        public SeoDataDto? SeoData { get; set; }
    }

    /// <summary>
    /// Представление платформы в списках (Item)
    /// </summary>
    public class PlatformOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SystemCode { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        // Количество связанных программ (Статистика)
        public int ProgramsCount { get; set; }

        // Название на текущем языке (для списков)
        public string? LocalizedName { get; set; }

        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Детальное представление платформы со всеми переводами и SEO
    /// </summary>
    public class PlatformOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SystemCode { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public List<PlatformOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// DTO для создания новой платформы
    /// </summary>
    public class PlatformOfAggregatorCreateDto
    {
        [Required]
        [MaxLength(PlatformOfAggregatorValidationConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(PlatformOfAggregatorValidationConstants.CodeMaxLength)]
        public string SystemCode { get; set; } = string.Empty;

        public string? IconPath { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public List<PlatformOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// DTO для обновления существующей платформы
    /// </summary>
    public class PlatformOfAggregatorUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(PlatformOfAggregatorValidationConstants.NameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(PlatformOfAggregatorValidationConstants.CodeMaxLength)]
        public string? SystemCode { get; set; }

        public string? IconPath { get; set; }
        public bool? IsActive { get; set; }
        public int? SortOrder { get; set; }

        public List<PlatformOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// Запрос страницы списка платформ
    /// </summary>
    public class PlatformOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }

        public PlatformOfAggregatorSortField SortBy { get; set; } = PlatformOfAggregatorSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;

        public bool ShowDeleted { get; set; }
    }

    /// <summary>
    /// Ответ с пагинацией для списка платформ
    /// </summary>
    public class PlatformOfAggregatorPagedResponseDto
    {
        public IEnumerable<PlatformOfAggregatorItemDto> Items { get; set; } = new List<PlatformOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Поля для сортировки платформ
    /// </summary>
    public enum PlatformOfAggregatorSortField
    {
        Id,
        Name,
        SystemCode,
        SortOrder,
        CreatedAt,
        UpdatedAt,
        ProgramsCount
    }
}
