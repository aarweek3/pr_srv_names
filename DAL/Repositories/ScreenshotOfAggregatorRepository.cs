using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ScreenshotOfAggregatorRepository : Repository<ScreenshotOfAggregator>, IScreenshotOfAggregatorRepository
    {
        public ScreenshotOfAggregatorRepository(AppDbContext context) : base(context) { }
    }
}
