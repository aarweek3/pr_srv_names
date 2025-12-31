using DAL.Models;
using System.ComponentModel.DataAnnotations;
using DAL.DTOs;
using DAL.Enums;

namespace pr_srv_names.Dtos
{
    /// <summary>
    /// DTO для создания пользователя (административное создание)
    /// </summary>
    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Department { get; set; }
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO для отображения результата поиска пользователей
    /// </summary>
    public class UserSearchResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO для выхода из системы
    /// </summary>
    public class LogoutDto
    {
        public string? RefreshToken { get; set; }
    }

    /// <summary>
    /// DTO для регистрации пользователя
    /// </summary>
    public class RegisterDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не короче 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать строчные и заглавные буквы, а также цифры")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для входа пользователя
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для передачи JWT access-токена
    /// </summary>
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    /// <summary>
    /// DTO для обновления refresh-токена
    /// </summary>
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Refresh-токен обязателен")]
        [MaxLength(500, ErrorMessage = "Refresh-токен не должен превышать 500 символов")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO ответа при успешной авторизации
    /// </summary>
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserProfileDto User { get; set; } = new();
        public bool RequiresTwoFactor { get; set; } = false;
    }

    /// <summary>
    /// DTO профиля пользователя
    /// </summary>
    public class UserProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Avatar { get; set; } // URL аватара
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsExternalAccount { get; set; }
        public string? ExternalProvider { get; set; }
        public string? ExternalId { get; set; }
    }

    /// <summary>
    /// DTO для обновления данных пользователя
    /// </summary>
    public class UpdateUserDto
    {
        [MaxLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        public string? FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
        public string? LastName { get; set; }

        [MaxLength(255, ErrorMessage = "URL аватара не должен превышать 255 символов")]
        [Url(ErrorMessage = "Некорректный формат URL аватара")]
        public string? Avatar { get; set; }

        [MaxLength(100, ErrorMessage = "Отдел не должен превышать 100 символов")]
        public string? Department { get; set; }
    }

    /// <summary>
    /// DTO для внешней авторизации (OAuth)
    /// </summary>
    public class ExternalLoginDto
    {
        [Required(ErrorMessage = "Провайдер авторизации обязателен")]
        [MaxLength(50, ErrorMessage = "Название провайдера не должно превышать 50 символов")]
        public string Provider { get; set; } = string.Empty;

        [Required(ErrorMessage = "Токен провайдера обязателен")]
        [MaxLength(1000, ErrorMessage = "Токен не должен превышать 1000 символов")]
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO для включения или отключения двухфакторной аутентификации
    /// </summary>
    public class TwoFactorDto
    {
        [Required(ErrorMessage = "Флаг 2FA обязателен")]
        public bool Enable { get; set; } // true — включить, false — отключить

        [MaxLength(6, ErrorMessage = "Код подтверждения не должен превышать 6 символов")]
        public string? VerificationCode { get; set; }
    }

    /// <summary>
    /// DTO для массовых операций над пользователями
    /// </summary>
    public class BulkOperationDto
    {
        [Required(ErrorMessage = "Список пользователей обязателен")]
        public List<string> UserIds { get; set; } = new List<string>();

        [Required(ErrorMessage = "Тип операции обязателен")]
        public BulkOperationType OperationType { get; set; }

        // Дополнительные параметры операции
        public string? NewRole { get; set; } // Для операции смены роли
        public string? Reason { get; set; }  // Причина операции
    }

    /// <summary>
    /// DTO пользовательской сессии
    /// </summary>
    public class UserSessionDto
    {
        public int Id { get; set; } // Внутренний идентификатор сессии

        [MaxLength(500, ErrorMessage = "Refresh-токен не должен превышать 500 символов")]
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        [MaxLength(500, ErrorMessage = "Информация об устройстве не должна превышать 500 символов")]
        public string? DeviceInfo { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO для обновления данных пользовательской сессии
    /// </summary>
    public class UpdateSessionDto
    {
        [MaxLength(500, ErrorMessage = "Refresh-токен не должен превышать 500 символов")]
        public string? RefreshToken { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public bool? IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        [MaxLength(500, ErrorMessage = "Информация об устройстве не должна превышать 500 символов")]
        public string? DeviceInfo { get; set; }
    }

    /// <summary>
    /// DTO записи журнала активности
    /// </summary>
    public class ActivityLogDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public ActivityAction Action { get; set; }
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? Details { get; set; }
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; }
        public DeviceType DeviceType { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    /// <summary>
    /// DTO фильтрации журнала активности
    /// </summary>
    public class ActivityLogFilterDto : BasePagedRequest
    {
        public string? UserId { get; set; }
        public ActivityAction? Action { get; set; }
        public bool? Success { get; set; }
        public DeviceType? DeviceType { get; set; }
        public string? EntityType { get; set; }
    }

    /// <summary>
    /// DTO для смены пароля
    /// </summary>
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Текущий пароль обязателен")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не короче 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать строчные и заглавные буквы, а также цифры")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO запроса восстановления пароля
    /// </summary>
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO сброса пароля
    /// </summary>
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Токен обязателен")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не короче 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать строчные и заглавные буквы, а также цифры")]
        public string NewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Универсальный DTO для постраничных ответов
    /// </summary>
    public class PagedResponseDto<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }

    /// <summary>
    /// DTO элемента списка пользователей
    /// </summary>
    public class UserListItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsExternalAccount { get; set; }
        public string? ExternalProvider { get; set; }
        public string? ExternalId { get; set; }
    }

    /// <summary>
    /// DTO фильтрации списка пользователей
    /// </summary>
    public class UserFilterDto : BasePagedRequest
    {
        public bool? IsActive { get; set; }
        public string? Department { get; set; }
        public bool? IsExternalAccount { get; set; }
        public string? ExternalProvider { get; set; }
    }

    /// <summary>
    /// DTO статистики по пользователям
    /// </summary>
    public class UserStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int ExternalAccounts { get; set; }
        public int UsersWithTwoFactor { get; set; }
        public Dictionary<string, int> UsersByDepartment { get; set; } = new();
        public List<DailyRegistrationDto> RegistrationTrend { get; set; } = new();
    }

    /// <summary>
    /// DTO статистики регистраций по дням
    /// </summary>
    public class DailyRegistrationDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
