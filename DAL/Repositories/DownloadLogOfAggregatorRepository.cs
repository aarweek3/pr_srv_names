using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class DownloadLogOfAggregatorRepository : Repository<DownloadLogOfAggregator>, IDownloadLogOfAggregatorRepository
    {
        public DownloadLogOfAggregatorRepository(AppDbContext context) : base(context) { }
    }
}
