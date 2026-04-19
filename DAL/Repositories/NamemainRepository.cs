using DAL.Models.NameModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class NameMainRepository : Repository<NameMain>, INameMainRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public NameMainRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NameMain>> GetAllAsync()
        {
            return await _context.Names
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<NameMain>> GetNameMainsWithDescriptionAsync()
        {
            return await GetQueryable()
                .Where(s => !string.IsNullOrEmpty(s.Description))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<NameMain?> GetNameMainByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await GetFirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsNameMainNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            Expression<Func<NameMain, bool>> predicate = s => s.Name.ToLower() == name.ToLower();

            if (excludeId.HasValue)
            {
                predicate = s => s.Name.ToLower() == name.ToLower() && s.Id != excludeId.Value;
            }

            return !await AnyAsync(predicate);
        }

        public async Task<IEnumerable<NameMain>> SearchCategoriesByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<NameMain>();

            var normalizedSearchTerm = searchTerm.ToLower();
            return await FindAsync(s => s.Name.ToLower().Contains(normalizedSearchTerm));
        }
    }
}