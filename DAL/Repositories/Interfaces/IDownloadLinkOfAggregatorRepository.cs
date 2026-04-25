using DAL.Models.Aggregator;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDownloadLinkOfAggregatorRepository : IRepository<DownloadLinkOfAggregator>
    {
        Task<DownloadLinkOfAggregator?> GetWithLocalizationsAsync(int id);
    }
}
