using pr_srv_names.Models;
using pr_srv_names.Pages.SampleMain.Models;

namespace pr_srv_names.Pages.SampleMain.Dtos
{
    /// <summary>
    /// Описание (перевод) для SampleMain
    /// </summary>
    public class SampleMainDescriptionDto
    {
        public int LanguageAppId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для создания новой многоязычной записи SampleMain
    /// </summary>
    public class SampleMainCreateRequestDto
    {
        public string Name { get; set; } = string.Empty; // Техническое имя
        public string? SystemCode { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Коллекция переводов
        public List<SampleMainDescriptionDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// DTO для обновления существующей многоязычной записи SampleMain
    /// </summary>
    public class SampleMainUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public bool IsActive { get; set; }
        
        public List<SampleMainDescriptionDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// DTO для краткого представления (списки)
    /// </summary>
    public class SampleMainItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Техническое имя
        public string? SystemCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Перевод для текущего выбранного языка (или Default)
        public string? LocalizedName { get; set; } 
    }

    /// <summary>
    /// Детальный DTO со всеми переводами
    /// </summary>
    public class SampleMainDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? SystemCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<SampleMainDescriptionDto> Descriptions { get; set; } = new();
    }

    /// <summary>
    /// Параметры запроса списка с пагинацией
    /// </summary>
    public class SampleMainPageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? LanguageId { get; set; } // Для какого языка искать/выводить LocalizedName

        public SampleMainSortField SortBy { get; set; } = SampleMainSortField.Name;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    /// <summary>
    /// Ответ сервера с пагинацией
    /// </summary>
    public class SampleMainPagedResponseDto
    {
        public IEnumerable<SampleMainItemDto> Items { get; set; } = new List<SampleMainItemDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
