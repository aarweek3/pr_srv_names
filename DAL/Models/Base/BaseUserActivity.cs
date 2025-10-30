using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Base;

/// <summary>
/// Базовая модель для активности пользователя
/// Используется для логирования действий и сессий пользователей
/// </summary>
/// <remarks>
/// Использует int Id для совместимости с существующими моделями
/// и для лучшей производительности при больших объемах данных
/// </remarks>
public abstract class BaseUserActivity
{
    /// <summary>
    /// Уникальный идентификатор записи активности
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя из AspNetUsers
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP адрес пользователя
    /// </summary>
    [MaxLength(45)] // Поддержка IPv6
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent браузера/устройства пользователя
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }
}