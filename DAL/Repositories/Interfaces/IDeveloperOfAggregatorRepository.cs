using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с разработчиками агрегатора.
    /// </summary>
    public interface IDeveloperOfAggregatorRepository : IRepository<DeveloperOfAggregator>
    {
        /// <summary>
        /// Получить разработчика со всеми локализациями и SEO-данными.
        /// </summary>
        Task<DeveloperOfAggregator?> GetWithDescriptionsAndSeoAsync(int id);

        /// <summary>
        /// Проверить уникальность SystemCode.
        /// </summary>
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);

        /// <summary>
        /// Проверить уникальность Name.
        /// </summary>
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    }
}
