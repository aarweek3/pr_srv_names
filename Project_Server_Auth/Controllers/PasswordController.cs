// Controllers/PasswordController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using pr_srv_names.Dtos;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PasswordController> _logger;

        public PasswordController(
            UserManager<ApplicationUser> userManager,
            ILogger<PasswordController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Неверные данные" });

                var user = await _userManager.FindByEmailAsync(dto.Email);

                // Всегда возвращаем успех (не раскрываем существование email)
                if (user == null)
                {
                    _logger.LogWarning("Попытка сброса пароля для несуществующего email: {Email}", dto.Email);
                    return Ok(new
                    {
                        success = true,
                        message = "Если email существует, письмо с инструкциями отправлено"
                    });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                _logger.LogInformation("Токен сброса пароля создан для {Email}", user.Email);

                // TODO: Отправить email через EmailService
                // await _emailService.SendPasswordResetEmailAsync(user.Email, token);

                return Ok(new
                {
                    success = true,
                    message = "Если email существует, письмо с инструкциями отправлено",
                    // ТОЛЬКО ДЛЯ РАЗРАБОТКИ - УДАЛИТЬ В PROD:
                    debugToken = token,
                    debugEmail = user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе сброса пароля");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest(new { success = false, message = errors });
                }

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    return BadRequest(new { success = false, message = "Неверные данные для сброса пароля" });

                var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { success = false, message = errors });
                }

                _logger.LogInformation("Пароль успешно сброшен для {Email}", user.Email);

                return Ok(new { success = true, message = "Пароль успешно изменен" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сбросе пароля");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }
    }
}