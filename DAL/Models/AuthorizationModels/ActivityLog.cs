using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using DAL.Enums;
using DAL.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.AuthorizationModels;

/// <summary>
/// Класс для логирования действий пользователя
/// Наследуется от BaseUserActivity и содержит информацию о типе действия,
/// связанной сущности, результате выполнения и устройстве пользователя
/// </summary>
[Table("ActivityLogs")]
[Index(nameof(UserId))]
[Index(nameof(Action))]
[Index(nameof(Timestamp))]
[Index(nameof(CreatedAt))]
public class ActivityLog : BaseUserActivity
{
    /// <summary>
    /// Тип действия пользователя
    /// Определяет, какое именно действие было выполнено
    /// </summary>
    [Required]
    public ActivityAction Action { get; set; }

    /// <summary>
    /// Тип сущности, связанной с действием
    /// Примеры: "User", "Name", "Comment", "Language"
    /// </summary>
    [StringLength(StringLengths.Name)]
    public string? EntityType { get; set; }

    /// <summary>
    /// Идентификатор сущности, с которой произведено действие
    /// Хранится как строка для универсальности
    /// </summary>
    [StringLength(StringLengths.ShortName)]
    public string? EntityId { get; set; }

    /// <summary>
    /// Дополнительные детали действия
    /// Может содержать JSON с подробной информацией или текстовое описание
    /// </summary>
    [StringLength(StringLengths.ShortDescription)]
    public string? Details { get; set; }

    /// <summary>
    /// Флаг успешного выполнения действия
    /// True - действие выполнено успешно, False - произошла ошибка
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Время выполнения действия
    /// Используется для точной временной метки события
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Тип устройства, с которого выполнено действие
    /// Определяет, использовался ли Desktop, Mobile или Tablet
    /// </summary>
    public DeviceType DeviceType { get; set; } = DeviceType.Unknown;

    // ==========================================
    // НАВИГАЦИОННЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Навигационное свойство для связи с пользователем
    /// Позволяет получить полную информацию о пользователе, выполнившем действие
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; } = null!;

    // ==========================================
    // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Проверяет, является ли действие критичным для безопасности
    /// </summary>
    [NotMapped]
    public bool IsSecurityCritical => Action is
        ActivityAction.Login or
        ActivityAction.Logout or
        ActivityAction.ForgotPassword or
        ActivityAction.ResetPassword or
        ActivityAction.ChangePassword or
        ActivityAction.BlockUser or
        ActivityAction.DeleteUser;

    /// <summary>
    /// Проверяет, связано ли действие с внешней аутентификацией
    /// </summary>
    [NotMapped]
    public bool IsExternalAuthAction => Action is
        ActivityAction.ExternalLogin or
        ActivityAction.LinkExternalAccount or
        ActivityAction.UnlinkExternalAccount or
        ActivityAction.ExternalRegister;

    /// <summary>
    /// Проверяет, является ли действие административным
    /// </summary>
    [NotMapped]
    public bool IsAdminAction => Action is
        ActivityAction.CreateUser or
        ActivityAction.UpdateUser or
        ActivityAction.BlockUser or
        ActivityAction.UnblockUser or
        ActivityAction.DeleteUser or
        ActivityAction.ViewLogs;

    /// <summary>
    /// Возвращает форматированное описание действия для отображения
    /// </summary>
    [NotMapped]
    public string FormattedAction => Action switch
    {
        ActivityAction.Login => "Вход в систему",
        ActivityAction.Logout => "Выход из системы",
        ActivityAction.Register => "Регистрация",
        ActivityAction.ForgotPassword => "Запрос восстановления пароля",
        ActivityAction.ResetPassword => "Сброс пароля",
        ActivityAction.ChangePassword => "Изменение пароля",
        ActivityAction.UpdateProfile => "Обновление профиля",
        ActivityAction.CreateUser => "Создание пользователя",
        ActivityAction.UpdateUser => "Обновление пользователя",
        ActivityAction.BlockUser => "Блокировка пользователя",
        ActivityAction.UnblockUser => "Разблокировка пользователя",
        ActivityAction.DeleteUser => "Удаление пользователя",
        ActivityAction.UpdateSettings => "Обновление настроек",
        ActivityAction.ViewLogs => "Просмотр логов",
        ActivityAction.ExternalLogin => "Вход через внешний сервис",
        ActivityAction.LinkExternalAccount => "Привязка внешнего аккаунта",
        ActivityAction.UnlinkExternalAccount => "Отвязка внешнего аккаунта",
        ActivityAction.ExternalRegister => "Регистрация через внешний сервис",
        _ => Action.ToString()
    };

    /// <summary>
    /// Возвращает иконку для типа действия (для UI)
    /// </summary>
    [NotMapped]
    public string ActionIcon => Action switch
    {
        ActivityAction.Login => "🔓",
        ActivityAction.Logout => "🔒",
        ActivityAction.Register => "📝",
        ActivityAction.ForgotPassword => "❓",
        ActivityAction.ResetPassword => "🔑",
        ActivityAction.ChangePassword => "🔐",
        ActivityAction.UpdateProfile => "👤",
        ActivityAction.CreateUser => "➕",
        ActivityAction.UpdateUser => "✏️",
        ActivityAction.BlockUser => "🚫",
        ActivityAction.UnblockUser => "✅",
        ActivityAction.DeleteUser => "🗑️",
        ActivityAction.UpdateSettings => "⚙️",
        ActivityAction.ViewLogs => "📊",
        ActivityAction.ExternalLogin => "🌐",
        ActivityAction.LinkExternalAccount => "🔗",
        ActivityAction.UnlinkExternalAccount => "⛓️",
        ActivityAction.ExternalRegister => "🌐",
        _ => "📌"
    };

    /// <summary>
    /// Возвращает CSS класс для стилизации (для UI)
    /// </summary>
    [NotMapped]
    public string StatusClass => Success ? "success" : "error";
}