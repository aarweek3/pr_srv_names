using DAL.Models.SampleModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SampleMainRepository : Repository<SampleMain>, ISampleMainRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public SampleMainRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            
            var query = _context.SamplesMain.AsNoTracking();
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }
            
            return !await query.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsSystemCodeUniqueAsync(string systemCode, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(systemCode)) return true; // Считаем уникальным, если не задан

            var query = _context.SamplesMain.AsNoTracking();

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return !await query.AnyAsync(x => x.SystemCode!.ToLower() == systemCode.ToLower());
        }

        public async Task<SampleMain?> GetWithDescriptionsAsync(int id)
        {
            return await _context.SamplesMain
                .Include(x => x.Descriptions)
                .ThenInclude(d => d.LanguageApp)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
