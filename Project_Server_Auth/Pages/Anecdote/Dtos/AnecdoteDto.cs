using pr_srv_names.Models;
using pr_srv_names.Pages.Anecdote.Models;

namespace pr_srv_names.Pages.Anecdote.Dtos
{
    /// <summary>
    /// Базовый DTO для параметров пагинации анекдотов
    /// </summary>
    public class AnecdoteBasePageRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO для создания нового анекдота
    /// </summary>
    public class AnecdoteCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NameMainId { get; set; }
        public int LanguageId { get; set; }
        public bool IsActive { get; set; } = true; // Добавлено поле
    }

    /// <summary>
    /// DTO для обновления существующего анекдота
    /// </summary>
    public class AnecdoteUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NameMainId { get; set; }
        public int LanguageId { get; set; }
        public bool IsActive { get; set; } = true; // Добавлено поле
    }

    /// <summary>
    /// DTO для детального представления анекдота
    /// </summary>
    public class AnecdoteDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NameMainId { get; set; }
        public string? NameMainName { get; set; }
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO для параметров пагинации и фильтрации анекдотов
    /// </summary>
    public class AnecdotePageRequestDto : AnecdoteBasePageRequestDto
    {
        public string? SearchTerm { get; set; }
        public int? NameMainId { get; set; }
        public int? LanguageId { get; set; }

        /// <summary>
        /// Поле сортировки
        /// </summary>
        public AnecdoteSortField SortBy { get; set; } = AnecdoteSortField.Name;

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    /// <summary>
    /// DTO для ответа с пагинацией анекдотов
    /// </summary>
    public class AnecdotePagedResponseDto
    {
        public IEnumerable<AnecdoteDetailDto> Items { get; set; } = new List<AnecdoteDetailDto>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // DTO для контрола
    /// <summary>
    /// DTO для получения списка (упрощенная версия)
    /// </summary>
    public class AnecdoteControlDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для создания через контрол
    /// </summary>
    public class AnecdoteControlCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public int NameMainId { get; set; }
        public int LanguageId { get; set; }
    }
}