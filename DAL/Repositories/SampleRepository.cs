using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class SampleRepository : Repository<Sample>, ISampleRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public SampleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sample>> GetAllAsync()
        {
            return await _context.Samples
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sample>> GetSamplesWithDescriptionAsync()
        {
            return await GetQueryable()
                .Where(s => !string.IsNullOrEmpty(s.Description))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Sample?> GetSampleByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await GetFirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsSampleNameUniqueAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            Expression<Func<Sample, bool>> predicate = s => s.Name.ToLower() == name.ToLower();

            if (excludeId.HasValue)
            {
                predicate = s => s.Name.ToLower() == name.ToLower() && s.Id != excludeId.Value;
            }

            return !await AnyAsync(predicate);
        }

        public async Task<IEnumerable<Sample>> SearchCategoriesByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Sample>();

            var normalizedSearchTerm = searchTerm.ToLower();
            return await FindAsync(s => s.Name.ToLower().Contains(normalizedSearchTerm));
        }
    }
}