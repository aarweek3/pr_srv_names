using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Типы массовых операций над пользователями
/// Используется для групповых действий в административной панели
/// </summary>
public enum BulkOperationType
{
    /// <summary>
    /// Массовая блокировка пользователей
    /// </summary>
    [Display(Name = "Блокировка")] Block,

    /// <summary>
    /// Массовая разблокировка пользователей
    /// </summary>
    [Display(Name = "Разблокировка")] Unblock,

    /// <summary>
    /// Массовое удаление пользователей
    /// </summary>
    [Display(Name = "Удаление")] Delete,

    /// <summary>
    /// Массовое изменение роли пользователей
    /// </summary>
    [Display(Name = "Изменение роли")] ChangeRole,

    /// <summary>
    /// Принудительный выход из всех сессий
    /// </summary>
    [Display(Name = "Принудительный выход")]
    ForceLogout,

    /// <summary>
    /// Массовый сброс паролей
    /// </summary>
    [Display(Name = "Сброс пароля")] ResetPassword,

    /// <summary>
    /// Массовое подтверждение email
    /// </summary>
    [Display(Name = "Подтверждение email")]
    ConfirmEmail
}