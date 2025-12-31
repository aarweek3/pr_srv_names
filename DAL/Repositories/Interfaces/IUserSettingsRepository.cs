using DAL.Enums.Settings;
using DAL.Models;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с настройками пользователей
    /// </summary>
    public interface IUserSettingsRepository : IRepository<UserSettings>
    {
        /// <summary>
        /// Получение настроек пользователя по UserId
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Настройки пользователя или null</returns>
        Task<UserSettings?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Получение настроек пользователя по UserId без отслеживания (для чтения)
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Настройки пользователя или null</returns>
        Task<UserSettings?> GetByUserIdAsNoTrackingAsync(string userId);

        /// <summary>
        /// Проверка существования настроек для пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>True если настройки существуют</returns>
        Task<bool> ExistsByUserIdAsync(string userId);

        /// <summary>
        /// Удаление настроек пользователя по UserId
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>True если удаление успешно</returns>
        Task<bool> DeleteByUserIdAsync(string userId);

        /// <summary>
        /// Получение всех пользователей с определённой темой
        /// </summary>
        /// <param name="theme">Тема интерфейса</param>
        /// <returns>Коллекция настроек</returns>
        Task<IEnumerable<UserSettings>> GetByThemeAsync(UiTheme theme);

        /// <summary>
        /// Получение количества пользователей с определённым языком
        /// </summary>
        /// <param name="language">Код языка</param>
        /// <returns>Количество пользователей</returns>
        Task<int> CountByLanguageAsync(string language);
    }
}
