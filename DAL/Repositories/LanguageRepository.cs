using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public LanguageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            return await _context.Languages
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Language>> GetLanguagesWithDescriptionAsync()
        {
            return await GetQueryable()
                .Where(s => !string.IsNullOrEmpty(s.Description))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Language?> GetLanguageByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await GetFirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsLanguageNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            Expression<Func<Language, bool>> predicate = s => s.Name.ToLower() == name.ToLower();

            if (excludeId.HasValue)
            {
                predicate = s => s.Name.ToLower() == name.ToLower() && s.Id != excludeId.Value;
            }

            return !await AnyAsync(predicate);
        }

        public async Task<bool> IsLanguageCodeUniqueAsync(string code, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            Expression<Func<Language, bool>> predicate = s => s.Code.ToLower() == code.ToLower();

            if (excludeId.HasValue)
            {
                predicate = s => s.Code.ToLower() == code.ToLower() && s.Id != excludeId.Value;
            }

            return !await AnyAsync(predicate);
        }

        public async Task<IEnumerable<Language>> SearchCategoriesByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Language>();

            var normalizedSearchTerm = searchTerm.ToLower();
            return await FindAsync(s => s.Name.ToLower().Contains(normalizedSearchTerm) ||
                                       s.Code.ToLower().Contains(normalizedSearchTerm));
        }
    }
}