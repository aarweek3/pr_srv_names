using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs;

/// <summary>
/// Базовый запрос с пагинацией и фильтрацией
/// Используется для всех API endpoints, возвращающих списки данных
/// </summary>
public class BasePagedRequest
{
    /// <summary>
    /// Номер страницы (начиная с 1)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Номер страницы должен быть больше 0")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Размер страницы (количество элементов)
    /// </summary>
    [Range(1, 1000, ErrorMessage = "Размер страницы должен быть от 1 до 1000")]
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Поисковый запрос для фильтрации результатов
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Поле для сортировки
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Направление сортировки (true = по убыванию, false = по возрастанию)
    /// </summary>
    public bool SortDescending { get; set; } = true;

    /// <summary>
    /// Начальная дата для фильтрации по диапазону дат
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Конечная дата для фильтрации по диапазону дат
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Вычисляет количество элементов для пропуска (для Skip в LINQ)
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Проверяет, валиден ли диапазон дат
    /// </summary>
    public bool IsDateRangeValid()
    {
        if (StartDate.HasValue && EndDate.HasValue)
        {
            return StartDate.Value <= EndDate.Value;
        }

        return true;
    }
}

/// <summary>
/// Базовый ответ для пагинированных данных
/// </summary>
public class PagedResponse<T>
{
    /// <summary>
    /// Список элементов текущей страницы
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Общее количество элементов
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Текущая страница
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Размер страницы
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Общее количество страниц
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Есть ли предыдущая страница
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Есть ли следующая страница
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}