using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class AggregatorSourceRepository : Repository<AggregatorSource>, IAggregatorSourceRepository
    {
        public AggregatorSourceRepository(AppDbContext context) : base(context) { }
    }
}
