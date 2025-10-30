using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace pr_srv_names.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var jwtSettings = ValidateJwtSettings(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                ConfigureJwtBearerOptions(options, jwtSettings, environment);
            });

            return services;
        }

        private static JwtSettings ValidateJwtSettings(IConfiguration configuration)
        {
            var jwtKey = configuration["JwtSettings:SecretKey"];
            var issuer = configuration["JwtSettings:Issuer"];
            var audience = configuration["JwtSettings:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
            {
                throw new InvalidOperationException("JWT ключ отсутствует или слишком короткий (минимум 32 символа)");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                throw new InvalidOperationException("JWT Issuer не указан в конфигурации");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("JWT Audience не указан в конфигурации");
            }

            var accessTokenLifetime = configuration.GetValue<int>("JwtSettings:AccessTokenLifetimeMinutes");
            var refreshTokenLifetime = configuration.GetValue<int>("JwtSettings:RefreshTokenLifetimeDays");

            return new JwtSettings
            {
                SecretKey = jwtKey,
                Issuer = issuer,
                Audience = audience,
                AccessTokenLifetimeMinutes = accessTokenLifetime > 0 ? accessTokenLifetime : 15,
                RefreshTokenLifetimeDays = refreshTokenLifetime > 0 ? refreshTokenLifetime : 7
            };
        }

        private static void ConfigureJwtBearerOptions(JwtBearerOptions options, JwtSettings jwtSettings, IWebHostEnvironment environment)
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = !environment.IsDevelopment();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                // Дополнительные настройки безопасности
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateActor = false,
                ValidateTokenReplay = false
            };

            ConfigureJwtBearerEvents(options);
        }

        private static void ConfigureJwtBearerEvents(JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    // Приоритет: Authorization header
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                        logger?.LogDebug("JWT токен получен из Authorization header для пути: {Path}", context.Request.Path);
                        return Task.CompletedTask;
                    }

                    // Fallback: HttpOnly cookie
                    var accessToken = context.Request.Cookies["accessToken"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                        logger?.LogDebug("JWT токен получен из HttpOnly cookie для пути: {Path}", context.Request.Path);
                    }
                    else
                    {
                        logger?.LogWarning("JWT токен не найден ни в header, ни в cookie для пути: {Path}", context.Request.Path);
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    logger?.LogWarning("JWT Authentication failed: {Error} для пути: {Path}",
                        context.Exception.Message, context.Request.Path);

                    // Специальная обработка истёкших токенов
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                        logger?.LogInformation("JWT токен истёк для пути: {Path}", context.Request.Path);
                    }

                    // Обработка невалидных токенов
                    if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
                    {
                        context.Response.Headers.Add("Token-Invalid-Signature", "true");
                        logger?.LogWarning("JWT токен имеет неверную подпись для пути: {Path}", context.Request.Path);
                    }

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var roles = context.Principal?.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToArray();
                    var userName = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

                    logger?.LogDebug("JWT токен успешно валидирован для пользователя: {UserId} ({UserName}), роли: [{Roles}]",
                        userId, userName, string.Join(", ", roles ?? Array.Empty<string>()));

                    // Дополнительные проверки токена
                    var jti = context.Principal?.FindFirst("jti")?.Value;
                    if (!string.IsNullOrEmpty(jti))
                    {
                        logger?.LogDebug("JWT ID: {JwtId}", jti);
                    }

                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    logger?.LogDebug("JWT Challenge инициирован для пути: {Path}, схема: {Scheme}, ошибка: {Error}",
                        context.Request.Path, context.Scheme, context.Error);

                    return Task.CompletedTask;
                },

                OnForbidden = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    logger?.LogWarning("JWT Forbidden для пути: {Path}, пользователь аутентифицирован: {IsAuthenticated}",
                        context.Request.Path, context.HttpContext.User.Identity?.IsAuthenticated ?? false);

                    return Task.CompletedTask;
                }
            };
        }

        public static IServiceCollection AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = ValidateJwtSettings(configuration);
            services.AddSingleton(jwtSettings);
            return services;
        }

        public static IServiceCollection ConfigureJwtCookies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = isDevelopment ? SameSiteMode.Lax : SameSiteMode.Strict;
                options.Secure = isDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            });

            return services;
        }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenLifetimeMinutes { get; set; } = 15;
        public int RefreshTokenLifetimeDays { get; set; } = 7;
        public bool RequireHttpsMetadata { get; set; } = true;
        public bool SaveToken { get; set; } = true;
        public TimeSpan ClockSkew { get; set; } = TimeSpan.Zero;
    }
}