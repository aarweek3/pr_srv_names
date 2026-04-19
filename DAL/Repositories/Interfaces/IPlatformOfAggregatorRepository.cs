using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с платформами агрегатора.
    /// Поддерживает специфические операции для SEO-сущностей.
    /// </summary>
    public interface IPlatformOfAggregatorRepository : IRepository<PlatformOfAggregator>
    {
        /// <summary>
        /// Получить платформу со всеми описаниями и SEO-данными.
        /// </summary>
        Task<PlatformOfAggregator?> GetWithDescriptionsAndSeoAsync(int id);

        /// <summary>
        /// Проверить уникальность системного кода.
        /// </summary>
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);

        /// <summary>
        /// Проверить уникальность публичного названия.
        /// </summary>
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    }
}
