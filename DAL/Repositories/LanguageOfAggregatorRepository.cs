using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с языками агрегатора.
    /// </summary>
    public class LanguageOfAggregatorRepository : Repository<LanguageOfAggregator>, ILanguageOfAggregatorRepository
    {
        public LanguageOfAggregatorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            var query = Entities.AsNoTracking().Where(x => x.Code == code);
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync();
        }

        public async Task<bool> IsShortCodeUniqueAsync(string shortCode, int? excludeId = null)
        {
            var query = Entities.AsNoTracking().Where(x => x.ShortCode == shortCode);
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync();
        }

        public async Task<LanguageOfAggregator?> GetDefaultLanguageAsync()
        {
            return await Entities.FirstOrDefaultAsync(x => x.IsDefault && x.Enabled);
        }
    }
}
