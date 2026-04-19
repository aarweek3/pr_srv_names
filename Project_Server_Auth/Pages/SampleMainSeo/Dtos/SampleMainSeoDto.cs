using pr_srv_names.Pages.Shared.Seo.Dtos;
using System.Collections.Generic;
using System;

namespace pr_srv_names.Pages.SampleMainSeo.Dtos
{
    /// <summary>
    /// Описание (перевод) для SampleMainSeo с вложенными SEO данными
    /// </summary>
    public class SampleMainDescriptionSeoDto
    {
        public int LanguageAppId { get; set; }
        public string? LanguageCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        /// <summary>
        /// HTML контент (статья/полное описание)
        /// </summary>
        public string? HtmlContent { get; set; }

        /// <summary>
        /// Локализованное изображение
        /// </summary>
        public string? UrlPicture { get; set; }
        
        /// <summary>
        /// Универсальные SEO данные (Композиция)
        /// </summary>
        public SeoDataDto? SeoData { get; set; }
    }

    /// <summary>
    /// DTO для создания новой записи SampleMainSeo
    /// </summary>
    public class SampleMainSeoCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public string? UrlPictureMain { get; set; }
        public bool IsActive { get; set; } = true;
        
        public List<SampleMainDescriptionSeoDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// DTO для обновления существующей записи SampleMainSeo
    /// </summary>
    public class SampleMainSeoUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public string? UrlPictureMain { get; set; }
        public bool IsActive { get; set; }
        
        public List<SampleMainDescriptionSeoDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// DTO для краткого представления в списках
    /// </summary>
    public class SampleMainSeoItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public string? UrlPictureMain { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public string? LocalizedName { get; set; } 
        public int SeoScore { get; set; }
    }

    /// <summary>
    /// Детальный DTO со всеми переводами и SEO данными
    /// </summary>
    public class SampleMainSeoDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public string? UrlPictureMain { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<SampleMainDescriptionSeoDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// Параметры запроса списка с пагинацией для SampleMainSeo
    /// </summary>
    public class SampleMainSeoPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; }

        public SampleMainSeoSortField SortBy { get; set; } = SampleMainSeoSortField.Name;
        public pr_srv_names.Models.SortDirection SortDirection { get; set; } = pr_srv_names.Models.SortDirection.Asc;
    }

    /// <summary>
    /// Ответ сервера с пагинацией для SampleMainSeo
    /// </summary>
    public class SampleMainSeoPagedResponseDto
    {
        public IEnumerable<SampleMainSeoItemDto> Items { get; set; } = new List<SampleMainSeoItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Поля для сортировки SampleMainSeo
    /// </summary>
    public enum SampleMainSeoSortField
    {
        Id,
        Name,
        SystemCode,
        CreatedAt,
        UpdatedAt,
        LocalizedName
    }
}
