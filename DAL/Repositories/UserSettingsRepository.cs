using DAL.Enums.Settings;
using DAL.Models.AuthorizationModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с настройками пользователей
    /// </summary>
    public class UserSettingsRepository : Repository<UserSettings>, IUserSettingsRepository
    {
        public UserSettingsRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Получение настроек пользователя по UserId с отслеживанием (для редактирования)
        /// </summary>
        public async Task<UserSettings?> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            return await GetFirstOrDefaultAsync(s => s.UserId == userId);
        }

        /// <summary>
        /// Получение настроек пользователя по UserId без отслеживания (для чтения)
        /// </summary>
        public async Task<UserSettings?> GetByUserIdAsNoTrackingAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        /// <summary>
        /// Проверка существования настроек для пользователя
        /// </summary>
        public async Task<bool> ExistsByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            return await AnyAsync(s => s.UserId == userId);
        }

        /// <summary>
        /// Удаление настроек пользователя по UserId
        /// </summary>
        public async Task<bool> DeleteByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var settings = await GetByUserIdAsync(userId);
            if (settings == null)
                return false;

            Delete(settings);
            return true;
        }

        /// <summary>
        /// Получение всех пользователей с определённой темой
        /// </summary>
        public async Task<IEnumerable<UserSettings>> GetByThemeAsync(UiTheme theme)
        {
            return await Entities
                .AsNoTracking()
                .Where(s => s.Theme == theme)
                .ToListAsync();
        }

        /// <summary>
        /// Получение количества пользователей с определённым языком
        /// </summary>
        public async Task<int> CountByLanguageAsync(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return 0;

            return await CountAsync(s => s.Language == language);
        }
    }
}
