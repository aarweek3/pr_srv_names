using pr_srv_names.Models;
using pr_srv_names.Pages.NameMain.Models;

namespace pr_srv_names.Pages.NameMain.Dtos

{
    /// <summary>
    /// Базовый DTO для параметров пагинации namemain.
    /// </summary>
    public class NameMainBasePageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO для создания новой namemain.
    /// </summary>
    public class NameMainCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующей namemain.
    /// </summary>
    public class NameMainUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для детального представления namemain.
    /// </summary>
    public class NameMainDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для параметров пагинации и фильтрации namemain.
    /// </summary>
    public class NameMainPageRequestDto : NameMainBasePageRequestDto
    {
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Поле сортировки. Допустимые значения: Name, Description, Id
        /// </summary>
        public NameMainSortField SortBy { get; set; } = NameMainSortField.Name;

        /// <summary>
        /// Направление сортировки. Допустимые значения: Asc, Desc
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    /// <summary>
    /// DTO для ответа с пагинацией namemain.
    /// </summary>
    public class NameMainPagedResponseDto
    {
        public IEnumerable<NameMainDetailDto> Items { get; set; } = new List<NameMainDetailDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // DTO контрол

    // DTO для получения списка
    public class NameMainControlDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // DTO для получения детальной информации (для примера)
    public class NameMainControlDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // DTO для создания
    public class NameMainControlCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}