using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;

namespace DAL.Models;

/// <summary>
/// Класс пользователя, расширяющий IdentityUser для поддержки кастомных полей
/// </summary>
public class ApplicationUser : IdentityUser
{
    // ==========================================
    // ОСНОВНАЯ ИНФОРМАЦИЯ О ПОЛЬЗОВАТЕЛЕ
    // ==========================================

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required]
    [StringLength(StringLengths.Name)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    [Required]
    [StringLength(StringLengths.Name)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// URL аватара пользователя (рекомендуется облачное хранение)
    /// </summary>
    [StringLength(StringLengths.Url)]
    public string? Avatar { get; set; }

    /// <summary>
    /// Отдел пользователя
    /// </summary>
    [StringLength(StringLengths.Name)]
    public string? Department { get; set; }

    /// <summary>
    /// Флаг активности пользователя
    /// </summary>
    public bool IsActive { get; set; } = true;

    // ==========================================
    // ВРЕМЕННЫЕ МЕТКИ
    // ==========================================

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время последнего входа
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Дата и время последнего обновления
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Дата и время подтверждения email
    /// </summary>
    public DateTime? EmailConfirmedAt { get; set; }

    // ==========================================
    // ДВУХФАКТОРНАЯ АУТЕНТИФИКАЦИЯ
    // ==========================================
    // ПРИМЕЧАНИЕ: Используем базовое свойство TwoFactorEnabled из IdentityUser
    // Оно уже есть в базовом классе и правильно интегрировано с Identity
    // Если нужна дополнительная логика, используйте NotMapped свойства ниже

    /// <summary>
    /// Проверяет, включена ли двухфакторная аутентификация
    /// Использует базовое свойство из IdentityUser
    /// </summary>
    [NotMapped]
    public bool Is2FAEnabled
    {
        get => TwoFactorEnabled;
        set => TwoFactorEnabled = value;
    }

    // ==========================================
    // OAUTH И ВНЕШНЯЯ АУТЕНТИФИКАЦИЯ
    // ==========================================

    /// <summary>
    /// Имя внешнего провайдера (Google, Facebook, и т.д.)
    /// </summary>
    [StringLength(StringLengths.ShortName)]
    public string? ExternalProvider { get; set; }

    /// <summary>
    /// Внешний идентификатор пользователя от провайдера
    /// </summary>
    [StringLength(StringLengths.Name)]
    public string? ExternalId { get; set; }

    /// <summary>
    /// Флаг, указывающий, является ли аккаунт внешним
    /// </summary>
    public bool IsExternalAccount { get; set; } = false;

    // ==========================================
    // НАВИГАЦИОННЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Коллекция сессий пользователя
    /// </summary>
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    /// <summary>
    /// Коллекция логов активности пользователя
    /// </summary>
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    // ==========================================
    // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Полное имя пользователя (Имя + Фамилия)
    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Проверка наличия пароля (учитывает внешние аккаунты)
    /// </summary>
    [NotMapped]
    public bool HasPassword => !IsExternalAccount || !string.IsNullOrEmpty(PasswordHash);

    /// <summary>
    /// Проверяет, подтверждён ли email пользователя
    /// </summary>
    [NotMapped]
    public bool IsEmailVerified => EmailConfirmedAt.HasValue;

    /// <summary>
    /// Количество дней с момента регистрации
    /// </summary>
    [NotMapped]
    public int DaysSinceRegistration => (DateTime.UtcNow - CreatedAt).Days;

    /// <summary>
    /// Количество активных сессий
    /// </summary>
    [NotMapped]
    public int ActiveSessionsCount => UserSessions?.Count(s => s.IsActive) ?? 0;
}