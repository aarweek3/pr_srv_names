using DAL.Models.SampleModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SampleMainSeoRepository : Repository<SampleMainSeo>, ISampleMainSeoRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public SampleMainSeoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            
            var query = _context.SamplesMainSeo.AsNoTracking();
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }
            
            return !await query.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsSystemCodeUniqueAsync(string systemCode, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(systemCode)) return true;

            var query = _context.SamplesMainSeo.AsNoTracking();

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return !await query.AnyAsync(x => x.SystemCode!.ToLower() == systemCode.ToLower());
        }

        public async Task<SampleMainSeo?> GetWithDescriptionsAndSeoAsync(int id)
        {
            return await _context.SamplesMainSeo
                .Include(x => x.Descriptions)
                    .ThenInclude(d => d.LanguageApp)
                .Include(x => x.Descriptions)
                    .ThenInclude(d => d.SeoData)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
