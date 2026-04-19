using pr_srv_names.Models;
using pr_srv_names.Pages.Sample.Models;

namespace pr_srv_names.Pages.Sample.Dtos

{
    /// <summary>
    /// Базовый DTO для параметров пагинации sample.
    /// </summary>
    public class SampleBasePageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO для создания новой sample.
    /// </summary>
    public class SampleCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующей sample.
    /// </summary>
    public class SampleUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для детального представления sample.
    /// </summary>
    public class SampleDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO для параметров пагинации и фильтрации sample.
    /// </summary>
    public class SamplePageRequestDto : SampleBasePageRequestDto
    {
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Поле сортировки. Допустимые значения: Name, Description, Id
        /// </summary>
        public SampleSortField SortBy { get; set; } = SampleSortField.Name;

        /// <summary>
        /// Направление сортировки. Допустимые значения: Asc, Desc
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    /// <summary>
    /// DTO для ответа с пагинацией sample.
    /// </summary>
    public class SamplePagedResponseDto
    {
        public IEnumerable<SampleDetailDto> Items { get; set; } = new List<SampleDetailDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // DTO контрол

    // DTO для получения списка
    public class SampleControlDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // DTO для получения детальной информации (для примера)
    public class SampleControlDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // DTO для создания
    public class SampleControlCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}