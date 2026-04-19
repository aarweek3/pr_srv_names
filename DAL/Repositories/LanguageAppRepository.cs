using DAL.Models.LocalizationModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LanguageAppRepository : Repository<LanguageApp>, ILanguageAppRepository
    {
        public LanguageAppRepository(AppDbContext context) : base(context)
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

        public async Task<LanguageApp?> GetDefaultLanguageAsync()
        {
            return await Entities.FirstOrDefaultAsync(x => x.IsDefault && x.Enabled);
        }
    }
}
