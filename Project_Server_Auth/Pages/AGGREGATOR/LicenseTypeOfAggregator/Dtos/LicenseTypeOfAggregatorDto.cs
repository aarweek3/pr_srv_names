using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Constants;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos
{
    /// <summary>
    /// Локализация для типа лицензии агрегатора
    /// </summary>
    public class LicenseTypeOfAggregatorLocalizationDto
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
    /// Представление типа лицензии в списках
    /// </summary>
    public class LicenseTypeOfAggregatorItemDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public string? LocalizedName { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Детальное представление со всеми локализациями
    /// </summary>
    public class LicenseTypeOfAggregatorDetailDto
    {
        public int Id { get; set; }
        public string CanonicalName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public List<LicenseTypeOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// DTO для создания
    /// </summary>
    public class LicenseTypeOfAggregatorCreateDto
    {
        [Required]
        [MaxLength(LicenseTypeOfAggregatorValidationConstants.NameMaxLength)]
        public string CanonicalName { get; set; } = string.Empty;

        [Required]
        [MaxLength(LicenseTypeOfAggregatorValidationConstants.CodeMaxLength)]
        public string Slug { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public List<LicenseTypeOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// DTO для обновления
    /// </summary>
    public class LicenseTypeOfAggregatorUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(LicenseTypeOfAggregatorValidationConstants.NameMaxLength)]
        public string? CanonicalName { get; set; }

        [MaxLength(LicenseTypeOfAggregatorValidationConstants.CodeMaxLength)]
        public string? Slug { get; set; }

        public bool? IsActive { get; set; }
        public int? SortOrder { get; set; }

        public List<LicenseTypeOfAggregatorLocalizationDto> Localizations { get; set; } = new();
    }

    /// <summary>
    /// Запрос страницы
    /// </summary>
    public class LicenseTypeOfAggregatorPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }

        public LicenseTypeOfAggregatorSortField SortBy { get; set; } = LicenseTypeOfAggregatorSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;

        public bool ShowDeleted { get; set; }
    }

    /// <summary>
    /// Ответ с пагинацией
    /// </summary>
    public class LicenseTypeOfAggregatorPagedResponseDto
    {
        public IEnumerable<LicenseTypeOfAggregatorItemDto> Items { get; set; } = new List<LicenseTypeOfAggregatorItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Поля для сортировки
    /// </summary>
    public enum LicenseTypeOfAggregatorSortField
    {
        Id,
        CanonicalName,
        Slug,
        SortOrder,
        CreatedAt,
        UpdatedAt
    }
}
