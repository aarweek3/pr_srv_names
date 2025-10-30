using pr_srv_names.Pages.Anecdote.Dtos;

namespace pr_srv_names.Pages.Anecdote.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с анекдотами
    /// </summary>
    public interface IAnecdoteService
    {
        /// <summary>
        /// Получить анекдот по идентификатору
        /// </summary>
        Task<AnecdoteDetailDto?> GetAnecdoteByIdAsync(int id);

        /// <summary>
        /// Получить список анекдотов с пагинацией и фильтрацией
        /// </summary>
        Task<AnecdotePagedResponseDto> GetAllAnecdotesAsync(AnecdotePageRequestDto request);

        /// <summary>
        /// Создать новый анекдот
        /// </summary>
        Task<AnecdoteDetailDto> CreateAnecdoteAsync(AnecdoteCreateRequestDto request);

        /// <summary>
        /// Обновить существующий анекдот
        /// </summary>
        Task<AnecdoteDetailDto> UpdateAnecdoteAsync(int id, AnecdoteUpdateRequestDto request);

        /// <summary>
        /// Удалить анекдот по идентификатору
        /// </summary>
        Task<bool> DeleteAnecdoteAsync(int id);

        /// <summary>
        /// Получить все анекдоты для конкретного имени
        /// </summary>
        Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByNameMainIdAsync(int nameMainId);

        /// <summary>
        /// Получить все анекдоты на конкретном языке
        /// </summary>
        Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByLanguageIdAsync(int languageId);

        /// <summary>
        /// Получить анекдоты по имени и языку
        /// </summary>
        Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByNameAndLanguageAsync(int nameMainId, int languageId);

        /// <summary>
        /// Поиск анекдотов по части названия
        /// </summary>
        Task<IEnumerable<AnecdoteDetailDto>> SearchAnecdotesByNameAsync(string searchTerm);

        /// <summary>
        /// Проверить существование анекдота по идентификатору
        /// </summary>
        Task<bool> AnecdoteExistsAsync(int id);

        /// <summary>
        /// Получить все анекдоты (для селектора)
        /// </summary>
        Task<IEnumerable<AnecdoteDetailDto>> GetAllAnecdotesAsync();
    }
}
