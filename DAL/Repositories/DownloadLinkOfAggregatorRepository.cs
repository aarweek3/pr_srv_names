using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DownloadLinkOfAggregatorRepository : Repository<DownloadLinkOfAggregator>, IDownloadLinkOfAggregatorRepository
    {
        public DownloadLinkOfAggregatorRepository(AppDbContext context) : base(context) { }

        public async Task<DownloadLinkOfAggregator?> GetWithLocalizationsAsync(int id)
        {
            return await Entities
                .Include(x => x.Localizations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
