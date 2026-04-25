using DAL.Models.Aggregator;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с типами лицензий агрегатора.
    /// </summary>
    public class LicenseTypeOfAggregatorRepository : Repository<LicenseTypeOfAggregator>, ILicenseTypeOfAggregatorRepository
    {
        public LicenseTypeOfAggregatorRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private AppDbContext _appContext => (AppDbContext)Context;

        /// <summary>
        /// Получить тип лицензии со всеми описаниями и SEO-данными.
        /// </summary>
        public async Task<LicenseTypeOfAggregator?> GetWithDescriptionsAndSeoAsync(int id)
        {
            return await _appContext.LicenseTypesOfAggregator
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.LanguageOfAggregator)
                .Include(x => x.Localizations)
                    .ThenInclude(l => l.SeoData)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Проверить уникальность Slug (системного кода).
        /// </summary>
        public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null)
        {
            return !await _appContext.LicenseTypesOfAggregator
                .AnyAsync(x => x.Slug == slug && (excludeId == null || x.Id != excludeId));
        }

        /// <summary>
        /// Проверить уникальность CanonicalName (публичного названия).
        /// </summary>
        public async Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await _appContext.LicenseTypesOfAggregator
                .AnyAsync(x => x.CanonicalName == name && (excludeId == null || x.Id != excludeId));
        }
    }
}
