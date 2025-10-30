using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Base;

/// <summary>
/// Базовая модель с аудитом (для моделей с Guid Id)
/// Используется для сущностей, требующих отслеживания IP и User Agent
/// </summary>
public abstract class BaseAuditableEntity
{
    /// <summary>
    /// Уникальный идентификатор сущности (GUID)
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP адрес, с которого была создана запись
    /// </summary>
    [MaxLength(45)] // Поддержка IPv6
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent браузера/устройства
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }
}