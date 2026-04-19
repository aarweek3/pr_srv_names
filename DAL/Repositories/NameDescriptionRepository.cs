using DAL.Models.NameModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с описаниями имен
    /// </summary>
    public class NameDescriptionRepository : Repository<NameDetail>, INameDescriptionRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public NameDescriptionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<NameDetail?> GetByNameAndLanguageAsync(int nameId, int languageId)
        {
            return await _context.NameDetails
                .Include(nd => nd.Name)
                .Include(nd => nd.Language)
                .FirstOrDefaultAsync(nd => nd.NameMainId == nameId && nd.LanguageId == languageId);
        }

        public async Task<IEnumerable<NameDetail>> GetByNameIdAsync(int nameId)
        {
            return await _context.NameDetails
                .Include(nd => nd.Language)
                .Where(nd => nd.NameMainId == nameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<NameDetail>> GetByLanguageIdAsync(int languageId)
        {
            return await _context.NameDetails
                .Include(nd => nd.Name)
                .Where(nd => nd.LanguageId == languageId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int nameId, int languageId)
        {
            return await _context.NameDetails
                .AnyAsync(nd => nd.NameMainId == nameId && nd.LanguageId == languageId);
        }

        public async Task DeleteByNameIdAsync(int nameId)
        {
            var descriptions = await _context.NameDetails
                .Where(nd => nd.NameMainId == nameId)
                .ToListAsync();

            _context.NameDetails.RemoveRange(descriptions);
        }
    }
}