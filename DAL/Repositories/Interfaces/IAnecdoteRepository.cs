using DAL.Models;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с анекдотами
    /// </summary>
    public interface IAnecdoteRepository : IRepository<Anecdote>
    {
        /// <summary>
        /// Получить анекдот по ID с включением связанных данных (NameMain, Language)
        /// </summary>
        Task<Anecdote?> GetByIdWithIncludesAsync(int id);

        /// <summary>
        /// Получить все анекдоты с включением связанных данных (NameMain, Language)
        /// </summary>
        Task<IEnumerable<Anecdote>> GetAllWithIncludesAsync();

        /// <summary>
        /// Получить анекдоты с пагинацией и включением связанных данных
        /// </summary>
        Task<(IEnumerable<Anecdote> Items, int Total)> GetPagedWithIncludesAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Anecdote, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<Anecdote, bool>>? filter = null);

        /// <summary>
        /// Получить все анекдоты для конкретного имени
        /// </summary>
        Task<IEnumerable<Anecdote>> GetByNameMainIdAsync(int nameMainId);

        /// <summary>
        /// Получить все анекдоты на конкретном языке
        /// </summary>
        Task<IEnumerable<Anecdote>> GetByLanguageIdAsync(int languageId);

        /// <summary>
        /// Получить анекдоты по имени и языку
        /// </summary>
        Task<IEnumerable<Anecdote>> GetByNameMainIdAndLanguageIdAsync(int nameMainId, int languageId);

        /// <summary>
        /// Поиск анекдотов по названию или описанию
        /// </summary>
        Task<IEnumerable<Anecdote>> SearchByTextAsync(string searchTerm);

        /// <summary>
        /// Получить количество анекдотов для конкретного имени
        /// </summary>
        Task<int> GetCountByNameMainIdAsync(int nameMainId);

        /// <summary>
        /// Получить количество анекдотов на конкретном языке
        /// </summary>
        Task<int> GetCountByLanguageIdAsync(int languageId);

        /// <summary>
        /// Проверить существование анекдота с заданными NameMainId и LanguageId
        /// </summary>
        Task<bool> ExistsByNameAndLanguageAsync(int nameMainId, int languageId, int? excludeId = null);

        /// <summary>
        /// Получить последние N анекдотов
        /// </summary>
        Task<IEnumerable<Anecdote>> GetLatestAsync(int count);

        /// <summary>
        /// Получить случайные анекдоты
        /// </summary>
        Task<IEnumerable<Anecdote>> GetRandomAnecdotesAsync(int count, int? nameMainId = null, int? languageId = null);

        /// <summary>
        /// Массовое удаление анекдотов по NameMainId
        /// </summary>
        Task<int> DeleteByNameMainIdAsync(int nameMainId);

        /// <summary>
        /// Массовое удаление анекдотов по LanguageId
        /// </summary>
        Task<int> DeleteByLanguageIdAsync(int languageId);
    }
}
