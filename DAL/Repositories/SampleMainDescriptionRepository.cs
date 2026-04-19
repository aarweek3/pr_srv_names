using DAL.Models.SampleModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SampleMainDescriptionRepository : Repository<SampleMainDescription>, ISampleMainDescriptionRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public SampleMainDescriptionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<SampleMainDescription?> GetTranslationAsync(int sampleMainId, int languageAppId)
        {
            return await _context.SamplesMainDescriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SampleMainId == sampleMainId && x.LanguageAppId == languageAppId);
        }

        public async Task<IEnumerable<SampleMainDescription>> GetByMainIdAsync(int sampleMainId)
        {
            return await _context.SamplesMainDescriptions
                .AsNoTracking()
                .Where(x => x.SampleMainId == sampleMainId)
                .Include(x => x.LanguageApp)
                .ToListAsync();
        }
    }
}
