using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с платформами агрегатора.
    /// </summary>
    public class PlatformOfAggregatorRepository : Repository<PlatformOfAggregator>, IPlatformOfAggregatorRepository
    {
        public PlatformOfAggregatorRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private AppDbContext _appContext => (AppDbContext)Context;

        /// <summary>
        /// Получить платформу со всеми описаниями и SEO-данными.
        /// </summary>
        public async Task<PlatformOfAggregator?> GetWithDescriptionsAndSeoAsync(int id)
        {
            return await _appContext.PlatformsOfAggregator
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.SeoData)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Проверить уникальность системного кода.
        /// </summary>
        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null)
        {
            return !await _appContext.PlatformsOfAggregator
                .AnyAsync(x => x.SystemCode == code && (excludeId == null || x.Id != excludeId));
        }

        /// <summary>
        /// Проверить уникальность публичного названия.
        /// </summary>
        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await _appContext.PlatformsOfAggregator
                .AnyAsync(x => x.Name == name && (excludeId == null || x.Id != excludeId));
        }
    }
}
