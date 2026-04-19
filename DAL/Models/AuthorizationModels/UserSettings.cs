using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using DAL.Enums.Settings;
using DAL.Interfaces.Settings;

namespace DAL.Models.AuthorizationModels;

/// <summary>
/// Сущность настроек профиля пользователя.
/// Реализует IUserSettings и добавляет свойства для БД (PK, FK).
/// </summary>
[Table("UserSettings")]
public class UserSettings : IUserSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }

    // ==========================================
    // 1. ВНЕШНИЙ ВИД (UI / UX)
    // ==========================================
    
    public UiTheme Theme { get; set; } = UiTheme.System;

    public UiDensity Density { get; set; } = UiDensity.Comfortable;

    [MaxLength(10)]
    public string? PrimaryColor { get; set; }


    // ==========================================
    // 2. НАВИГАЦИЯ И LAYOUT
    // ==========================================

    public SidebarState SidebarState { get; set; } = SidebarState.Expanded;

    public NavigationBehavior NavigationBehavior { get; set; } = NavigationBehavior.RememberLastPage;


    // ==========================================
    // 3. СПИСКИ И ТАБЛИЦЫ
    // ==========================================

    public TableDensity TableDensity { get; set; } = TableDensity.Normal;

    public DefaultPageSizeOption DefaultPageSize { get; set; } = DefaultPageSizeOption.Size10;

    public bool ShowAdvancedFilters { get; set; } = false;


    // ==========================================
    // 4. ЛОКАЛИЗАЦИЯ
    // ==========================================

    [MaxLength(10)]
    public string Language { get; set; } = "ru-RU";

    [MaxLength(50)]
    public string TimeZone { get; set; } = "UTC";


    // ==========================================
    // 5. ДОСТУПНОСТЬ
    // ==========================================

    public AccessibilityLevel AccessibilityLevel { get; set; } = AccessibilityLevel.Standard;


    // ==========================================
    // 6. УВЕДОМЛЕНИЯ
    // ==========================================

    public NotificationLevel NotificationLevel { get; set; } = NotificationLevel.All;

    public NotificationChannel NotificationChannels { get; set; } = NotificationChannel.Email | NotificationChannel.InApp;


    // ==========================================
    // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
    // ==========================================

    public SessionTerminationMode SessionTerminationMode { get; set; } = SessionTerminationMode.Manual;

    public LoginNotificationMode LoginNotificationMode { get; set; } = LoginNotificationMode.NewDeviceOnly;
}
