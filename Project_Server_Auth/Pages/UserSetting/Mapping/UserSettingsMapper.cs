using DAL.Models;
using pr_srv_names.Pages.UserSetting.Dtos;

namespace pr_srv_names.Pages.UserSetting.Mapping;

/// <summary>
/// Ручной маппер для преобразования UserSettings между Entity и DTO.
/// </summary>
public static class UserSettingsMapper
{
    /// <summary>
    /// Преобразование Entity в DetailDto (для чтения)
    /// </summary>
    /// <param name="entity">Сущность UserSettings</param>
    /// <returns>DTO для чтения</returns>
    public static UserSettingsDetailDto ToDetailDto(UserSettings entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        return new UserSettingsDetailDto
        {
            // 1. ВНЕШНИЙ ВИД (UI / UX)
            Theme = entity.Theme,
            Density = entity.Density,
            PrimaryColor = entity.PrimaryColor,

            // 2. НАВИГАЦИЯ И LAYOUT
            SidebarState = entity.SidebarState,
            NavigationBehavior = entity.NavigationBehavior,

            // 3. СПИСКИ И ТАБЛИЦЫ
            TableDensity = entity.TableDensity,
            DefaultPageSize = entity.DefaultPageSize,
            ShowAdvancedFilters = entity.ShowAdvancedFilters,

            // 4. ЛОКАЛИЗАЦИЯ
            Language = entity.Language,
            TimeZone = entity.TimeZone,

            // 5. ДОСТУПНОСТЬ
            AccessibilityLevel = entity.AccessibilityLevel,

            // 6. УВЕДОМЛЕНИЯ
            NotificationLevel = entity.NotificationLevel,
            NotificationChannels = entity.NotificationChannels,

            // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
            SessionTerminationMode = entity.SessionTerminationMode,
            LoginNotificationMode = entity.LoginNotificationMode
        };
    }

    /// <summary>
    /// Применение изменений из UpdateDto к существующей Entity
    /// </summary>
    /// <param name="entity">Существующая сущность UserSettings</param>
    /// <param name="dto">DTO с обновлёнными данными</param>
    public static void UpdateFromDto(UserSettings entity, UserSettingsUpdateDto dto)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        // 1. ВНЕШНИЙ ВИД (UI / UX)
        entity.Theme = dto.Theme;
        entity.Density = dto.Density;
        entity.PrimaryColor = dto.PrimaryColor;

        // 2. НАВИГАЦИЯ И LAYOUT
        entity.SidebarState = dto.SidebarState;
        entity.NavigationBehavior = dto.NavigationBehavior;

        // 3. СПИСКИ И ТАБЛИЦЫ
        entity.TableDensity = dto.TableDensity;
        entity.DefaultPageSize = dto.DefaultPageSize;
        entity.ShowAdvancedFilters = dto.ShowAdvancedFilters;

        // 4. ЛОКАЛИЗАЦИЯ
        entity.Language = dto.Language;
        entity.TimeZone = dto.TimeZone;

        // 5. ДОСТУПНОСТЬ
        entity.AccessibilityLevel = dto.AccessibilityLevel;

        // 6. УВЕДОМЛЕНИЯ
        entity.NotificationLevel = dto.NotificationLevel;
        entity.NotificationChannels = dto.NotificationChannels;

        // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
        entity.SessionTerminationMode = dto.SessionTerminationMode;
        entity.LoginNotificationMode = dto.LoginNotificationMode;
    }

    /// <summary>
    /// Создание новой Entity из UpdateDto (для создания дефолтных настроек)
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="dto">DTO с настройками (опционально, если null - используются дефолтные значения)</param>
    /// <returns>Новая сущность UserSettings</returns>
    public static UserSettings CreateFromDto(string userId, UserSettingsUpdateDto? dto = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        var entity = new UserSettings
        {
            UserId = userId
        };

        // Если DTO передан, применяем его значения, иначе используются дефолтные из модели
        if (dto != null)
        {
            UpdateFromDto(entity, dto);
        }

        return entity;
    }
}
