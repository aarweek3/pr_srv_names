using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProgramOfAggregatorRepository : Repository<ProgramOfAggregator>, IProgramOfAggregatorRepository
    {
        public ProgramOfAggregatorRepository(AppDbContext context) : base(context) { }

        public async Task<ProgramOfAggregator?> GetWithDetailsAsync(int id)
        {
            return await Entities
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .Include(x => x.ProgramPlatforms)
                    .ThenInclude(p => p.PlatformOfAggregator)
                .Include(x => x.MarketData)
                .Include(x => x.Versions)
                .Include(x => x.Screenshots)
                .Include(x => x.Videos)
                .Include(x => x.ProgramTags)
                    .ThenInclude(t => t.TagOfAggregator)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null)
        {
            return !await Entities.AnyAsync(x => x.Slug == slug && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity != null)
            {
                entity.Delete();
                Entities.Update(entity);
            }
        }

        public async Task RestoreAsync(int id)
        {
            var entity = await Entities.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.Restore();
                Entities.Update(entity);
            }
        }
    }
}
