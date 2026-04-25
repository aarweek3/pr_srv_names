using DAL.Models.Aggregator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DAL.Repositories.Interfaces
{
    public interface ICategoryOfAggregatorRepository : IRepository<CategoryOfAggregator>
    {
        Task<CategoryOfAggregator?> GetWithLocalizationsAsync(int id);
        Task<List<CategoryOfAggregator>> GetTreeAsync(int? languageId = null);
        Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
