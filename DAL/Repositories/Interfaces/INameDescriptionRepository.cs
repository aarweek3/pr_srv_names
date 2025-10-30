using DAL.Models;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с описаниями имен
    /// </summary>
    public interface INameDescriptionRepository : IRepository<NameDetail>
    {
        /// <summary>
        /// Получить описание по имени и языку
        /// </summary>
        Task<NameDetail?> GetByNameAndLanguageAsync(int nameId, int languageId);

        /// <summary>
        /// Получить все описания для конкретного имени
        /// </summary>
        Task<IEnumerable<NameDetail>> GetByNameIdAsync(int nameId);

        /// <summary>
        /// Получить все описания на конкретном языке
        /// </summary>
        Task<IEnumerable<NameDetail>> GetByLanguageIdAsync(int languageId);

        /// <summary>
        /// Проверить существование описания
        /// </summary>
        Task<bool> ExistsAsync(int nameId, int languageId);

        /// <summary>
        /// Удалить все описания для имени
        /// </summary>
        Task DeleteByNameIdAsync(int nameId);
    }
}