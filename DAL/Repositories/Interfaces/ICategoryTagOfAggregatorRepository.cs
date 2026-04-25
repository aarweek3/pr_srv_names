using DAL.Models.Aggregator;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для категорий тегов агрегатора.
    /// </summary>
    public interface ICategoryTagOfAggregatorRepository : IRepository<CategoryTagOfAggregator>
    {
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);
    }
}
