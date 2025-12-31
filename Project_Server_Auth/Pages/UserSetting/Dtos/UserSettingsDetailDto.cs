using DAL.Enums.Settings;

namespace pr_srv_names.Pages.UserSetting.Dtos;

/// <summary>
/// DTO для чтения настроек пользователя.
/// Используется для передачи данных на фронтенд.
/// </summary>
public class UserSettingsDetailDto
{
    // ==========================================
    // 1. ВНЕШНИЙ ВИД (UI / UX)
    // ==========================================
    
    /// <summary>
    /// Тема оформления интерфейса
    /// </summary>
    public UiTheme Theme { get; set; }

    /// <summary>
    /// Плотность элементов интерфейса
    /// </summary>
    public UiDensity Density { get; set; }

    /// <summary>
    /// Основной цвет (опционально, HEX)
    /// </summary>
    public string? PrimaryColor { get; set; }


    // ==========================================
    // 2. НАВИГАЦИЯ И LAYOUT
    // ==========================================

    /// <summary>
    /// Состояние бокового меню
    /// </summary>
    public SidebarState SidebarState { get; set; }

    /// <summary>
    /// Поведение навигации при входе
    /// </summary>
    public NavigationBehavior NavigationBehavior { get; set; }


    // ==========================================
    // 3. СПИСКИ И ТАБЛИЦЫ
    // ==========================================

    /// <summary>
    /// Плотность отображения таблиц
    /// </summary>
    public TableDensity TableDensity { get; set; }

    /// <summary>
    /// Количество записей на странице по умолчанию
    /// </summary>
    public DefaultPageSizeOption DefaultPageSize { get; set; }

    /// <summary>
    /// Показывать расширенные фильтры
    /// </summary>
    public bool ShowAdvancedFilters { get; set; }


    // ==========================================
    // 4. ЛОКАЛИЗАЦИЯ
    // ==========================================

    /// <summary>
    /// Язык интерфейса (например, "ru-RU", "en-US")
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Часовой пояс (например, "UTC", "Europe/Moscow")
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;


    // ==========================================
    // 5. ДОСТУПНОСТЬ
    // ==========================================

    /// <summary>
    /// Уровень доступности интерфейса
    /// </summary>
    public AccessibilityLevel AccessibilityLevel { get; set; }


    // ==========================================
    // 6. УВЕДОМЛЕНИЯ
    // ==========================================

    /// <summary>
    /// Уровень важности уведомлений
    /// </summary>
    public NotificationLevel NotificationLevel { get; set; }

    /// <summary>
    /// Каналы доставки уведомлений (Flags)
    /// </summary>
    public NotificationChannel NotificationChannels { get; set; }


    // ==========================================
    // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
    // ==========================================

    /// <summary>
    /// Режим завершения сессии
    /// </summary>
    public SessionTerminationMode SessionTerminationMode { get; set; }

    /// <summary>
    /// Режим уведомлений о входе
    /// </summary>
    public LoginNotificationMode LoginNotificationMode { get; set; }
}
