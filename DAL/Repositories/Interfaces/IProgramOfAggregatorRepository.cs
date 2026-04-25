using DAL.Models.Aggregator;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProgramOfAggregatorRepository : IRepository<ProgramOfAggregator>
    {
        Task<ProgramOfAggregator?> GetWithDetailsAsync(int id);
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
