using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Действия с доверенными устройствами
/// Используется для управления списком доверенных устройств пользователя
/// </summary>
public enum TrustedDeviceAction
{
    /// <summary>
    /// Удалить устройство из доверенных
    /// </summary>
    [Display(Name = "Удалить")] Remove,

    /// <summary>
    /// Переименовать устройство
    /// </summary>
    [Display(Name = "Переименовать")] Rename,

    /// <summary>
    /// Заблокировать устройство
    /// </summary>
    [Display(Name = "Заблокировать")] Block
}