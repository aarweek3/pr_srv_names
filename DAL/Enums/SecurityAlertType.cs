using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Типы предупреждений безопасности
/// Используется для классификации событий безопасности
/// </summary>
public enum SecurityAlertType
{
    /// <summary>
    /// Множественные неудачные попытки входа
    /// </summary>
    [Display(Name = "Множественные неудачные входы")]
    MultipleFailedLogins,

    /// <summary>
    /// Подозрительная геолокация
    /// </summary>
    [Display(Name = "Подозрительная локация")]
    SuspiciousLocation,

    /// <summary>
    /// Необычная активность
    /// </summary>
    [Display(Name = "Необычная активность")]
    UnusualActivity,

    /// <summary>
    /// Попытка брутфорс-атаки
    /// </summary>
    [Display(Name = "Попытка брутфорса")] BruteForceAttempt,

    /// <summary>
    /// Аккаунт заблокирован
    /// </summary>
    [Display(Name = "Аккаунт заблокирован")]
    AccountLocked,

    /// <summary>
    /// Запрос на сброс пароля
    /// </summary>
    [Display(Name = "Запрос сброса пароля")]
    PasswordResetRequest,

    /// <summary>
    /// Вход с нового устройства
    /// </summary>
    [Display(Name = "Новое устройство")] NewDeviceLogin
}