using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Методы двухфакторной аутентификации
/// Определяет способ получения второго фактора для входа
/// </summary>
public enum TwoFactorMethod
{
    /// <summary>
    /// Код подтверждения через SMS
    /// </summary>
    [Display(Name = "SMS")] SMS,

    /// <summary>
    /// Код подтверждения через Email
    /// </summary>
    [Display(Name = "Email")] Email,

    /// <summary>
    /// Приложение-аутентификатор (Google Authenticator, Authy)
    /// </summary>
    [Display(Name = "Приложение-аутентификатор")]
    AuthenticatorApp,

    /// <summary>
    /// Резервные коды восстановления
    /// </summary>
    [Display(Name = "Резервные коды")] BackupCodes
}