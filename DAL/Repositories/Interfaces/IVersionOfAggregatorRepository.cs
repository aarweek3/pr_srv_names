using DAL.Models.Aggregator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DAL.Repositories.Interfaces
{
    public interface IVersionOfAggregatorRepository : IRepository<VersionOfAggregator>
    {
        Task<VersionOfAggregator?> GetWithDetailsAsync(int id);
        Task<List<VersionOfAggregator>> GetByProgramIdAsync(int programId);
        Task<bool> IsVersionNumberUniqueAsync(int programId, string versionNumber, int? excludeId = null);
        Task<VersionOfAggregator?> GetLatestVersionAsync(int programId);
    }
}
