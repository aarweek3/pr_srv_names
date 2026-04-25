using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с типами лицензий агрегатора.
    /// Поддерживает специфические операции для SEO-сущностей.
    /// </summary>
    public interface ILicenseTypeOfAggregatorRepository : IRepository<LicenseTypeOfAggregator>
    {
        /// <summary>
        /// Получить тип лицензии со всеми описаниями и SEO-данными.
        /// </summary>
        Task<LicenseTypeOfAggregator?> GetWithDescriptionsAndSeoAsync(int id);

        /// <summary>
        /// Проверить уникальность Slug (системного кода).
        /// </summary>
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);

        /// <summary>
        /// Проверить уникальность CanonicalName (публичного названия).
        /// </summary>
        Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null);
    }
}
