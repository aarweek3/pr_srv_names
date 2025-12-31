using pr_srv_names.Pages.UserSetting.Dtos;

namespace pr_srv_names.Pages.UserSetting.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с настройками пользователей
    /// </summary>
    public interface IUserSettingsService
    {
        /// <summary>
        /// Получение настроек пользователя по UserId
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Настройки пользователя или null</returns>
        Task<UserSettingsDetailDto?> GetSettingsByUserIdAsync(string userId);

        /// <summary>
        /// Получение настроек текущего авторизованного пользователя
        /// </summary>
        /// <returns>Настройки пользователя</returns>
        Task<UserSettingsDetailDto> GetMySettingsAsync();

        /// <summary>
        /// Обновление настроек пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="dto">DTO с обновлёнными настройками</param>
        /// <returns>Обновлённые настройки</returns>
        Task<UserSettingsDetailDto> UpdateSettingsAsync(string userId, UserSettingsUpdateDto dto);

        /// <summary>
        /// Обновление настроек текущего авторизованного пользователя
        /// </summary>
        /// <param name="dto">DTO с обновлёнными настройками</param>
        /// <returns>Обновлённые настройки</returns>
        Task<UserSettingsDetailDto> UpdateMySettingsAsync(UserSettingsUpdateDto dto);

        /// <summary>
        /// Создание дефолтных настроек для нового пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Созданные настройки</returns>
        Task<UserSettingsDetailDto> CreateDefaultSettingsAsync(string userId);

        /// <summary>
        /// Сброс настроек пользователя к дефолтным значениям
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Сброшенные настройки</returns>
        Task<UserSettingsDetailDto> ResetToDefaultsAsync(string userId);

        /// <summary>
        /// Проверка существования настроек для пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>True если настройки существуют</returns>
        Task<bool> SettingsExistAsync(string userId);
    }
}
