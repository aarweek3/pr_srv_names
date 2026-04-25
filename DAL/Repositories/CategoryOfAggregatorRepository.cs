using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CategoryOfAggregatorRepository : Repository<CategoryOfAggregator>, ICategoryOfAggregatorRepository
    {
        public CategoryOfAggregatorRepository(AppDbContext context) : base(context) { }

        public async Task<CategoryOfAggregator?> GetWithLocalizationsAsync(int id)
        {
            return await Entities
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CategoryOfAggregator>> GetTreeAsync(int? languageId = null)
        {
            var query = Entities.AsQueryable();

            if (languageId.HasValue)
            {
                query = query.Include(x => x.Localizations.Where(l => l.LanguageOfAggregatorId == languageId.Value))
                    .ThenInclude(l => l.LanguageOfAggregator);
            }
            else
            {
                query = query.Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator);
            }

            return await query
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.CanonicalName)
                .ToListAsync();
        }

        public async Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await Entities.AnyAsync(x => x.CanonicalName == name && (!excludeId.HasValue || x.Id != excludeId.Value));
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
