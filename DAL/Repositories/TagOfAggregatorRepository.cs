using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TagOfAggregatorRepository : Repository<TagOfAggregator>, ITagOfAggregatorRepository
    {
        public TagOfAggregatorRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null)
        {
            return !await Entities.AnyAsync(x => x.Slug == slug && x.Id != excludeId);
        }
    }
}
