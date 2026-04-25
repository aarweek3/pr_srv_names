using DAL.Models.Aggregator;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для тегов агрегатора.
    /// </summary>
    public interface ITagOfAggregatorRepository : IRepository<TagOfAggregator>
    {
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);
    }
}
