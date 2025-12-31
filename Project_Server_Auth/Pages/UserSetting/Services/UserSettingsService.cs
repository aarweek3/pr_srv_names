using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.UserSetting.Dtos;
using pr_srv_names.Pages.UserSetting.Interfaces;
using pr_srv_names.Pages.UserSetting.Mapping;
using System.Security.Claims;

namespace pr_srv_names.Pages.UserSetting.Services
{
    /// <summary>
    /// Сервис для работы с настройками пользователей
    /// </summary>
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingsRepository _settingsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserSettingsService> _logger;
        private readonly IValidator<UserSettingsUpdateDto> _updateValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSettingsService(
            IUserSettingsRepository settingsRepository,
            IUnitOfWork unitOfWork,
            ILogger<UserSettingsService> logger,
            IValidator<UserSettingsUpdateDto> updateValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCorrelationId() => _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        private string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                var correlationId = GetCorrelationId();
                _logger.LogWarning("Не удалось получить UserId из контекста. CorrelationId: {CorrelationId}", correlationId);
                throw new UnauthorizedAccessException("Пользователь не авторизован.");
            }
            return userId;
        }

        private void ValidateUserId(string userId, string operationName)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                var correlationId = GetCorrelationId();
                _logger.LogWarning("Получен пустой UserId для {Operation}. CorrelationId: {CorrelationId}",
                    operationName, correlationId);
                throw new InvalidParametersException("Некорректный идентификатор пользователя.");
            }
        }

        /// <summary>
        /// Получение настроек или создание дефолтных, если их нет
        /// </summary>
        private async Task<DAL.Models.UserSettings> GetOrCreateSettingsAsync(string userId)
        {
            var correlationId = GetCorrelationId();
            var settings = await _settingsRepository.GetByUserIdAsync(userId);

            if (settings == null)
            {
                _logger.LogInformation("Настройки для пользователя {UserId} не найдены, создаём дефолтные. CorrelationId: {CorrelationId}",
                    userId, correlationId);

                settings = UserSettingsMapper.CreateFromDto(userId);
                await _settingsRepository.AddAsync(settings);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Дефолтные настройки для пользователя {UserId} созданы. CorrelationId: {CorrelationId}",
                    userId, correlationId);
            }

            return settings;
        }

        public async Task<UserSettingsDetailDto?> GetSettingsByUserIdAsync(string userId)
        {
            var correlationId = GetCorrelationId();
            ValidateUserId(userId, nameof(GetSettingsByUserIdAsync));

            try
            {
                var settings = await _settingsRepository.GetByUserIdAsNoTrackingAsync(userId);
                if (settings == null)
                {
                    _logger.LogWarning("Настройки для пользователя {UserId} не найдены. CorrelationId: {CorrelationId}",
                        userId, correlationId);
                    return null;
                }

                var result = UserSettingsMapper.ToDetailDto(settings);
                _logger.LogInformation("Настройки для пользователя {UserId} успешно получены. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении настроек пользователя {UserId}. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                throw;
            }
        }

        public async Task<UserSettingsDetailDto> GetMySettingsAsync()
        {
            var userId = GetCurrentUserId();
            var correlationId = GetCorrelationId();

            try
            {
                var settings = await GetOrCreateSettingsAsync(userId);
                var result = UserSettingsMapper.ToDetailDto(settings);

                _logger.LogInformation("Настройки текущего пользователя {UserId} успешно получены. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is UnauthorizedAccessException))
            {
                _logger.LogError(ex, "Ошибка при получении настроек текущего пользователя. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<UserSettingsDetailDto> UpdateSettingsAsync(string userId, UserSettingsUpdateDto dto)
        {
            var correlationId = GetCorrelationId();
            ValidateUserId(userId, nameof(UpdateSettingsAsync));

            if (dto == null)
            {
                _logger.LogWarning("Параметры запроса UpdateSettingsAsync отсутствуют. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Валидация UserSettingsUpdateDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var settings = await GetOrCreateSettingsAsync(userId);
                var originalTheme = settings.Theme;
                var originalLanguage = settings.Language;

                UserSettingsMapper.UpdateFromDto(settings, dto);
                _settingsRepository.Update(settings);
                await _unitOfWork.SaveChangesAsync();

                var result = UserSettingsMapper.ToDetailDto(settings);
                _logger.LogInformation("Настройки обновлены - UserId: {UserId}, Theme: {OldTheme} -> {NewTheme}, Language: {OldLang} -> {NewLang}. CorrelationId: {CorrelationId}",
                    userId, originalTheme, settings.Theme, originalLanguage, settings.Language, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при обновлении настроек пользователя {UserId}. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                throw;
            }
        }

        public async Task<UserSettingsDetailDto> UpdateMySettingsAsync(UserSettingsUpdateDto dto)
        {
            var userId = GetCurrentUserId();
            return await UpdateSettingsAsync(userId, dto);
        }

        public async Task<UserSettingsDetailDto> CreateDefaultSettingsAsync(string userId)
        {
            var correlationId = GetCorrelationId();
            ValidateUserId(userId, nameof(CreateDefaultSettingsAsync));

            try
            {
                var exists = await _settingsRepository.ExistsByUserIdAsync(userId);
                if (exists)
                {
                    _logger.LogWarning("Настройки для пользователя {UserId} уже существуют. CorrelationId: {CorrelationId}",
                        userId, correlationId);
                    throw new ConflictException($"Настройки для пользователя {userId} уже существуют.", "UserSettings", "UserId");
                }

                var settings = UserSettingsMapper.CreateFromDto(userId);
                await _settingsRepository.AddAsync(settings);
                await _unitOfWork.SaveChangesAsync();

                var result = UserSettingsMapper.ToDetailDto(settings);
                _logger.LogInformation("Дефолтные настройки для пользователя {UserId} успешно созданы. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is ConflictException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании дефолтных настроек для пользователя {UserId}. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                throw;
            }
        }

        public async Task<UserSettingsDetailDto> ResetToDefaultsAsync(string userId)
        {
            var correlationId = GetCorrelationId();
            ValidateUserId(userId, nameof(ResetToDefaultsAsync));

            try
            {
                var settings = await _settingsRepository.GetByUserIdAsync(userId);
                if (settings == null)
                {
                    _logger.LogWarning("Настройки для пользователя {UserId} не найдены для сброса. CorrelationId: {CorrelationId}",
                        userId, correlationId);
                    throw new NotFoundException("UserSettings", $"Настройки для пользователя {userId} не найдены.");
                }

                // Создаём дефолтные настройки и применяем их к существующей сущности
                var defaultDto = new UserSettingsUpdateDto
                {
                    Theme = DAL.Enums.Settings.UiTheme.System,
                    Density = DAL.Enums.Settings.UiDensity.Comfortable,
                    PrimaryColor = null,
                    SidebarState = DAL.Enums.Settings.SidebarState.Expanded,
                    NavigationBehavior = DAL.Enums.Settings.NavigationBehavior.RememberLastPage,
                    TableDensity = DAL.Enums.Settings.TableDensity.Normal,
                    DefaultPageSize = DAL.Enums.Settings.DefaultPageSizeOption.Size10,
                    ShowAdvancedFilters = false,
                    Language = "ru-RU",
                    TimeZone = "UTC",
                    AccessibilityLevel = DAL.Enums.Settings.AccessibilityLevel.Standard,
                    NotificationLevel = DAL.Enums.Settings.NotificationLevel.All,
                    NotificationChannels = DAL.Enums.Settings.NotificationChannel.Email | DAL.Enums.Settings.NotificationChannel.InApp,
                    SessionTerminationMode = DAL.Enums.Settings.SessionTerminationMode.Manual,
                    LoginNotificationMode = DAL.Enums.Settings.LoginNotificationMode.NewDeviceOnly
                };

                UserSettingsMapper.UpdateFromDto(settings, defaultDto);
                _settingsRepository.Update(settings);
                await _unitOfWork.SaveChangesAsync();

                var result = UserSettingsMapper.ToDetailDto(settings);
                _logger.LogInformation("Настройки для пользователя {UserId} успешно сброшены к дефолтным. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при сбросе настроек пользователя {UserId}. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                throw;
            }
        }

        public async Task<bool> SettingsExistAsync(string userId)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogDebug("Получен пустой UserId для SettingsExistAsync. Возвращаем false. CorrelationId: {CorrelationId}",
                    correlationId);
                return false;
            }

            try
            {
                var exists = await _settingsRepository.ExistsByUserIdAsync(userId);
                _logger.LogDebug("Проверка существования настроек для пользователя {UserId}: {Exists}. CorrelationId: {CorrelationId}",
                    userId, exists, correlationId);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке существования настроек для пользователя {UserId}. CorrelationId: {CorrelationId}",
                    userId, correlationId);
                return false;
            }
        }
    }
}
