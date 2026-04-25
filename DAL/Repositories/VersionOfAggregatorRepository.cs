using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class VersionOfAggregatorRepository : Repository<VersionOfAggregator>, IVersionOfAggregatorRepository
    {
        public VersionOfAggregatorRepository(AppDbContext context) : base(context) { }

        public async Task<VersionOfAggregator?> GetWithDetailsAsync(int id)
        {
            return await Entities
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .Include(x => x.DownloadLinks)
                    .ThenInclude(d => d.Localizations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<VersionOfAggregator>> GetByProgramIdAsync(int programId)
        {
            return await Entities
                .Where(x => x.ProgramOfAggregatorId == programId)
                .OrderByDescending(x => x.IsLatest)
                .ThenByDescending(x => x.ReleasedAt)
                .ThenByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<bool> IsVersionNumberUniqueAsync(int programId, string versionNumber, int? excludeId = null)
        {
            return !await Entities.AnyAsync(x => 
                x.ProgramOfAggregatorId == programId && 
                x.VersionNumber == versionNumber && 
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<VersionOfAggregator?> GetLatestVersionAsync(int programId)
        {
            return await Entities
                .Where(x => x.ProgramOfAggregatorId == programId && x.IsLatest)
                .FirstOrDefaultAsync();
        }
    }
}
