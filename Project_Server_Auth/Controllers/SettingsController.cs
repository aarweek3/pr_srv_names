using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.UserSetting.Dtos;
using pr_srv_names.Pages.UserSetting.Interfaces;
using System.Security.Claims;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления настройками пользователей
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly IUserSettingsService _settingsService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(
            IUserSettingsService settingsService,
            ILogger<SettingsController> logger)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получение настроек текущего пользователя
        /// </summary>
        /// <returns>Настройки пользователя</returns>
        /// <response code="200">Настройки успешно получены</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserSettingsDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMySettings()
        {
            try
            {
                var settings = await _settingsService.GetMySettingsAsync();
                return Ok(new { success = true, data = settings });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Попытка получения настроек неавторизованным пользователем");
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении настроек пользователя");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Обновление настроек текущего пользователя
        /// </summary>
        /// <param name="dto">DTO с обновлёнными настройками</param>
        /// <returns>Обновлённые настройки</returns>
        /// <response code="200">Настройки успешно обновлены</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut]
        [ProducesResponseType(typeof(UserSettingsDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMySettings([FromBody] UserSettingsUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Некорректные данные при обновлении настроек: {Errors}",
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return BadRequest(new { success = false, errors = ModelState });
                }

                var settings = await _settingsService.UpdateMySettingsAsync(dto);
                _logger.LogInformation("Настройки пользователя успешно обновлены");
                return Ok(new { success = true, data = settings });
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Валидация настроек не пройдена");
                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { success = false, errors });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Попытка обновления настроек неавторизованным пользователем");
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении настроек пользователя");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Частичное обновление настроек (PATCH)
        /// </summary>
        /// <param name="dto">DTO с частичными обновлениями</param>
        /// <returns>Обновлённые настройки</returns>
        /// <response code="200">Настройки успешно обновлены</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPatch]
        [ProducesResponseType(typeof(UserSettingsDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchMySettings([FromBody] UserSettingsUpdateDto dto)
        {
            // Для упрощения используем тот же метод, что и PUT
            // В будущем можно реализовать настоящий PATCH с JSON Patch
            return await UpdateMySettings(dto);
        }

        /// <summary>
        /// Сброс настроек к дефолтным значениям
        /// </summary>
        /// <returns>Сброшенные настройки</returns>
        /// <response code="200">Настройки успешно сброшены</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="404">Настройки не найдены</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("reset")]
        [ProducesResponseType(typeof(UserSettingsDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetToDefaults()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    _logger.LogWarning("Не удалось получить UserId из контекста при сбросе настроек");
                    return Unauthorized(new { success = false, message = "Пользователь не авторизован" });
                }

                var settings = await _settingsService.ResetToDefaultsAsync(userId);
                _logger.LogInformation("Настройки пользователя {UserId} успешно сброшены к дефолтным", userId);
                return Ok(new { success = true, data = settings });
            }
            catch (pr_srv_names.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, "Настройки не найдены при попытке сброса");
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сбросе настроек пользователя");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Получение настроек пользователя по UserId (только для администраторов)
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Настройки пользователя</returns>
        /// <response code="200">Настройки успешно получены</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Недостаточно прав</response>
        /// <response code="404">Настройки не найдены</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(UserSettingsDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserSettings(string userId)
        {
            try
            {
                var settings = await _settingsService.GetSettingsByUserIdAsync(userId);
                if (settings == null)
                {
                    _logger.LogWarning("Настройки для пользователя {UserId} не найдены", userId);
                    return NotFound(new { success = false, message = $"Настройки для пользователя {userId} не найдены" });
                }

                return Ok(new { success = true, data = settings });
            }
            catch (pr_srv_names.Exceptions.InvalidParametersException ex)
            {
                _logger.LogWarning(ex, "Некорректный UserId: {UserId}", userId);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении настроек пользователя {UserId}", userId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Проверка существования настроек для текущего пользователя
        /// </summary>
        /// <returns>True если настройки существуют</returns>
        /// <response code="200">Проверка выполнена успешно</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckSettingsExist()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { success = false, message = "Пользователь не авторизован" });
                }

                var exists = await _settingsService.SettingsExistAsync(userId);
                return Ok(new { success = true, exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке существования настроек");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }
    }
}
