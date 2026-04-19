// контроллер для внешней аутентификации
using DAL;
using DAL.Models;
using DAL.Models.AuthorizationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Services.Interfaces;
using System;
using System.Security.Claims;

namespace pr_srv_names.Controllers
{
    [Route("api/auth/external")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService; // Ваш сервис генерации JWT               
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public ExternalAuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }



        // Метод 1: Начало входа (вызывается с фронта)
        [HttpGet("login/{provider}")]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "ExternalAuth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider); // Отправляет на Google
        }

        // Метод 2: Возврат от провайдера
        [HttpGet("callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // Получаем информацию от провайдера
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return Redirect("http://localhost:4200/auth/login?error=ExternalAuthInfoNull");

            // Ищем пользователя по LoginProvider (Google/Facebook) и ProviderKey
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                // Пользователя нет - Ищем по email
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                
                // ВАЖНО: Если email не пришел от провайдера (например, Facebook не отдал)
                // Генерируем временный email, чтобы не блокировать вход
                if (string.IsNullOrEmpty(email))
                {
                    var providerKey = info.ProviderKey;
                    var cleanProvider = info.LoginProvider.ToLower();
                    email = $"{cleanProvider}_{providerKey}@placeholder.com";
                }

                user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    // Пользователя совсем нет - СОЗДАЕМ
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Unknown",
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User",
                        IsExternalAccount = true,
                        ExternalProvider = info.LoginProvider,
                        ExternalId = info.ProviderKey,
                        IsActive = true
                    };
                    
                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                         // Логируем или возвращаем ошибку, если создание не удалось
                         var errors = string.Join(",", createResult.Errors.Select(e => e.Description));
                         return Redirect($"http://localhost:4200/auth/login?error=CreateUserFailed&details={errors}");
                    }
                    
                    // Добавляем роль User по умолчанию
                    await _userManager.AddToRoleAsync(user, "User");
                }

                // Привязываем внешний логин к пользователю
                await _userManager.AddLoginAsync(user, info);
            }

            // ОБНОВЛЕНИЕ ПОЛЬЗОВАТЕЛЯ ПЕРЕД ГЕНЕРАЦИЕЙ ТОКЕНА
            // Важно перезагрузить пользователя, чтобы SecurityStamp и Роли точно подтянулись
            user = await _userManager.FindByIdAsync(user.Id);

            // Обновляем провайдера, если пользователь входит через другой провайдер
            if (!string.IsNullOrEmpty(user.ExternalProvider) && !string.IsNullOrEmpty(user.ExternalId))
            {
                if (user.ExternalProvider != info.LoginProvider || user.ExternalId != info.ProviderKey)
                {
                    user.ExternalProvider = info.LoginProvider;
                    user.ExternalId = info.ProviderKey;
                    user.IsExternalAccount = true;
                    await _userManager.UpdateAsync(user);
                }
            }
            else
            {
                user.ExternalProvider = info.LoginProvider;
                user.ExternalId = info.ProviderKey;
                user.IsExternalAccount = true;
                await _userManager.UpdateAsync(user);
            }

            // Генерируем НАШ токен
            var jwtToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Сохраняем рефреш токен в БД
            var session = new UserSession 
            { 
                UserId = user.Id, 
                RefreshToken = refreshToken, 
                ExpiresAt = DateTime.UtcNow.AddDays(30), 
                IsRevoked = false 
            };
            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();

            // Перенаправляем на фронтенд
            var encodedJwt =  Uri.EscapeDataString(jwtToken);
            var encodedRefresh = Uri.EscapeDataString(refreshToken);

            var frontendUrl = $"http://localhost:4200/auth/external-callback?token={encodedJwt}&refresh={encodedRefresh}";
            return Redirect(frontendUrl);
        }
    }
}
