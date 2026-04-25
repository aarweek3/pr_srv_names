using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class VideoOfAggregatorRepository : Repository<VideoOfAggregator>, IVideoOfAggregatorRepository
    {
        public VideoOfAggregatorRepository(AppDbContext context) : base(context) { }
    }
}
