using DAL.Enums.Settings;

namespace DAL.Interfaces.Settings;

/// <summary>
/// Интерфейс настроек пользователя, определяющий предпочтения UX, навигации и уведомлений.
/// </summary>
public interface IUserSettings
{
    // ==========================================
    // 1. ВНЕШНИЙ ВИД (UI / UX)
    // ==========================================
    
    /// <summary>
    /// Тема оформления интерфейса
    /// </summary>
    UiTheme Theme { get; set; }

    /// <summary>
    /// Плотность элементов интерфейса
    /// </summary>
    UiDensity Density { get; set; }

    /// <summary>
    /// Основной цвет (опционально, HEX)
    /// </summary>
    string? PrimaryColor { get; set; }


    // ==========================================
    // 2. НАВИГАЦИЯ И LAYOUT
    // ==========================================

    /// <summary>
    /// Состояние бокового меню
    /// </summary>
    SidebarState SidebarState { get; set; }

    /// <summary>
    /// Поведение навигации при входе
    /// </summary>
    NavigationBehavior NavigationBehavior { get; set; }


    // ==========================================
    // 3. СПИСКИ И ТАБЛИЦЫ
    // ==========================================

    /// <summary>
    /// Плотность отображения таблиц
    /// </summary>
    TableDensity TableDensity { get; set; }

    /// <summary>
    /// Количество записей на странице по умолчанию
    /// </summary>
    DefaultPageSizeOption DefaultPageSize { get; set; }

    /// <summary>
    /// Показывать расширенные фильтры
    /// </summary>
    bool ShowAdvancedFilters { get; set; }


    // ==========================================
    // 4. ЛОКАЛИЗАЦИЯ
    // ==========================================

    string Language { get; set; }
    string TimeZone { get; set; }


    // ==========================================
    // 5. ДОСТУПНОСТЬ
    // ==========================================

    /// <summary>
    /// Уровень доступности интерфейса
    /// </summary>
    AccessibilityLevel AccessibilityLevel { get; set; }


    // ==========================================
    // 6. УВЕДОМЛЕНИЯ
    // ==========================================

    /// <summary>
    /// Уровень важности уведомлений
    /// </summary>
    NotificationLevel NotificationLevel { get; set; }

    /// <summary>
    /// Каналы доставки уведомлений (Flags)
    /// </summary>
    NotificationChannel NotificationChannels { get; set; }


    // ==========================================
    // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
    // ==========================================

    /// <summary>
    /// Режим завершения сессии
    /// </summary>
    SessionTerminationMode SessionTerminationMode { get; set; }

    /// <summary>
    /// Режим уведомлений о входе
    /// </summary>
    LoginNotificationMode LoginNotificationMode { get; set; }
}
