using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с разработчиками агрегатора.
    /// </summary>
    public class DeveloperOfAggregatorRepository : Repository<DeveloperOfAggregator>, IDeveloperOfAggregatorRepository
    {
        public DeveloperOfAggregatorRepository(AppDbContext context) : base(context)
        {
        }

        private AppDbContext _appContext => (AppDbContext)Context;

        /// <summary>
        /// Получить разработчика со всеми локализациями и SEO-данными.
        /// </summary>
        public async Task<DeveloperOfAggregator?> GetWithDescriptionsAndSeoAsync(int id)
        {
            return await _appContext.DevelopersOfAggregator
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Проверить уникальность SystemCode.
        /// </summary>
        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null)
        {
            return !await _appContext.DevelopersOfAggregator
                .AnyAsync(x => x.SystemCode == code && (excludeId == null || x.Id != excludeId));
        }

        /// <summary>
        /// Проверить уникальность Name.
        /// </summary>
        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await _appContext.DevelopersOfAggregator
                .AnyAsync(x => x.Name == name && (excludeId == null || x.Id != excludeId));
        }
    }
}
