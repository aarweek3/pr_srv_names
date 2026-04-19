using pr_srv_names.Pages.Shared.Seo.Dtos;
using System.Collections.Generic;
using System;

namespace pr_srv_names.Pages.Platform.Dtos
{
    /// <summary>
    /// Описание (перевод) для платформы с вложенными SEO данными
    /// </summary>
    public class PlatformTranslationDto
    {
        public int LanguageId { get; set; }
        public string? LanguageCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DescriptionFull { get; set; }
        
        /// <summary>
        /// Локализованное изображение (переопределяет UrlPictureMain)
        /// </summary>
        public string? UrlPicture { get; set; }
        
        /// <summary>
        /// Универсальные SEO данные
        /// </summary>
        public SeoDataDto? SeoData { get; set; }
    }

    /// <summary>
    /// DTO для создания новой платформы
    /// </summary>
    public class PlatformCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Family { get; set; }
        
        /// <summary>
        /// Главное изображение (по умолчанию для всех языков)
        /// </summary>
        public string? UrlPictureMain { get; set; }
        
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 100;
        
        public List<PlatformTranslationDto> Translations { get; set; } = new();
    }

    /// <summary>
    /// DTO для обновления существующей платформы
    /// </summary>
    public class PlatformUpdateDto : PlatformCreateDto
    {
        public Guid Id { get; set; }
    }

    /// <summary>
    /// DTO для краткого представления в списках
    /// </summary>
    public class PlatformItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Family { get; set; }
        public string? UrlPictureMain { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        
        public string? LocalizedName { get; set; } 
    }

    /// <summary>
    /// Детальный DTO со всеми переводами и SEO данными
    /// </summary>
    public class PlatformDetailDto : PlatformItemDto
    {
        public List<PlatformTranslationDto> Translations { get; set; } = new();
    }

    /// <summary>
    /// Параметры запроса списка с пагинацией для платформ
    /// </summary>
    public class PlatformPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }

        public PlatformSortField SortBy { get; set; } = PlatformSortField.SortOrder;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
    }

    /// <summary>
    /// Ответ сервера с пагинацией для платформ
    /// </summary>
    public class PlatformPagedResponseDto
    {
        public IEnumerable<PlatformItemDto> Items { get; set; } = new List<PlatformItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Поля для сортировки платформ
    /// </summary>
    public enum PlatformSortField
    {
        Id,
        Name,
        Code,
        SortOrder,
        CreatedAt,
        UpdatedAt,
        LocalizedName
    }
}
