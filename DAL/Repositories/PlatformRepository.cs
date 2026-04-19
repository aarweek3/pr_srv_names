using DAL.Models.Business;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PlatformRepository : Repository<Platform>, IPlatformRepository
    {
        private AppDbContext _context => (AppDbContext)Context;

        public PlatformRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Platform?> GetWithTranslationsAndSeoAsync(Guid id)
        {
            return await Entities
                .Include(x => x.Translations)
                    .ThenInclude(t => t.Language)
                .Include(x => x.Translations)
                    .ThenInclude(t => t.SeoData)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null)
        {
            return !await Entities.AnyAsync(x => x.Code == code && (excludeId == null || x.Id != excludeId));
        }
    }
}
