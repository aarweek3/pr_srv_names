using pr_srv_names.Models;
using pr_srv_names.Pages.Language.Models;

namespace pr_srv_names.Pages.Language.Dtos
{
    /// <summary>
    /// Базовый DTO для параметров пагинации language.
    /// </summary>
    public class LanguageBasePageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO для создания новой language.
    /// </summary>
    public class LanguageCreateRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? FlagCode { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующей language.
    /// </summary>
    public class LanguageUpdateRequestDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? FlagCode { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для детального представления language.
    /// </summary>
    public class LanguageDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? FlagCode { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для параметров пагинации и фильтрации language.
    /// </summary>
    public class LanguagePageRequestDto : LanguageBasePageRequestDto
    {
        public string? SearchTerm { get; set; }
        /// <summary>
        /// Поле сортировки. Допустимые значения: Code, Name, Description, Id
        /// </summary>
        public LanguageSortField SortBy { get; set; } = LanguageSortField.Name;
        /// <summary>
        /// Направление сортировки. Допустимые значения: Asc, Desc
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    /// <summary>
    /// DTO для ответа с пагинацией language.
    /// </summary>
    public class LanguagePagedResponseDto
    {
        public IEnumerable<LanguageDetailDto> Items { get; set; } = new List<LanguageDetailDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // DTO для упрощенного представления (Control)
    public class LanguageControlDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    // DTO для детального представления (Control)
    public class LanguageControlDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? FlagCode { get; set; }
        public string? Description { get; set; }
    }

    // DTO для создания (Control)
    public class LanguageControlCreateDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? FlagCode { get; set; }
    }
}