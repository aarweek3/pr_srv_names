using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Типы устройств для идентификации источника активности
/// </summary>
public enum DeviceType
{
    /// <summary>
    /// Неизвестный тип устройства
    /// </summary>
    [Display(Name = "Неизвестно")] Unknown = 0,

    /// <summary>
    /// Настольный компьютер или ноутбук
    /// </summary>
    [Display(Name = "Компьютер")] Desktop = 1,

    /// <summary>
    /// Мобильный телефон
    /// </summary>
    [Display(Name = "Мобильный")] Mobile = 2,

    /// <summary>
    /// Планшет
    /// </summary>
    [Display(Name = "Планшет")] Tablet = 3
}