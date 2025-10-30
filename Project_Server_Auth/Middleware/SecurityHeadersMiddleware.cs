// Middleware/SecurityHeadersMiddleware.cs
namespace pr_srv_names.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityHeadersMiddleware> _logger;

        public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Добавляем заголовки безопасности
            AddSecurityHeaders(context.Response);

            // Удаляем информационные заголовки
            RemoveServerHeaders(context.Response);

            await _next(context);
        }

        private void AddSecurityHeaders(HttpResponse response)
        {
            // Предотвращение MIME-sniffing
            if (!response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                response.Headers.Add("X-Content-Type-Options", "nosniff");
            }

            // Защита от clickjacking
            if (!response.Headers.ContainsKey("X-Frame-Options"))
            {
                response.Headers.Add("X-Frame-Options", "DENY");
            }

            // XSS защита (для старых браузеров)
            if (!response.Headers.ContainsKey("X-XSS-Protection"))
            {
                response.Headers.Add("X-XSS-Protection", "1; mode=block");
            }

            // Контроль referrer
            if (!response.Headers.ContainsKey("Referrer-Policy"))
            {
                response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            }

            // Content Security Policy
            if (!response.Headers.ContainsKey("Content-Security-Policy"))
            {
                var csp = BuildContentSecurityPolicy();
                response.Headers.Add("Content-Security-Policy", csp);
            }

            // Permissions Policy (замена Feature-Policy)
            if (!response.Headers.ContainsKey("Permissions-Policy"))
            {
                var permissionsPolicy = BuildPermissionsPolicy();
                response.Headers.Add("Permissions-Policy", permissionsPolicy);
            }

            // HSTS (только для HTTPS)
            if (!response.Headers.ContainsKey("Strict-Transport-Security") &&
                response.HttpContext.Request.IsHttps)
            {
                response.Headers.Add("Strict-Transport-Security",
                    "max-age=31536000; includeSubDomains; preload");
            }

            // Предотвращение кеширования чувствительных данных
            if (IsSensitiveEndpoint(response.HttpContext.Request.Path))
            {
                response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, private");
                response.Headers.Add("Pragma", "no-cache");
                response.Headers.Add("Expires", "0");
            }
        }

        private string BuildContentSecurityPolicy()
        {
            var policies = new List<string>
            {
                "default-src 'self'",
                "script-src 'self' 'unsafe-inline'", // В продакшене убрать 'unsafe-inline'
                "style-src 'self' 'unsafe-inline'",
                "img-src 'self' data: https:",
                "font-src 'self'",
                "connect-src 'self'",
                "media-src 'none'",
                "object-src 'none'",
                "child-src 'none'",
                "frame-src 'none'",
                "worker-src 'none'",
                "frame-ancestors 'none'",
                "form-action 'self'",
                "base-uri 'self'",
                "manifest-src 'self'"
            };

            return string.Join("; ", policies);
        }

        private string BuildPermissionsPolicy()
        {
            var policies = new List<string>
            {
                "accelerometer=()",
                "ambient-light-sensor=()",
                "autoplay=()",
                "battery=()",
                "camera=()",
                "cross-origin-isolated=()",
                "display-capture=()",
                "document-domain=()",
                "encrypted-media=()",
                "execution-while-not-rendered=()",
                "execution-while-out-of-viewport=()",
                "fullscreen=()",
                "geolocation=()",
                "gyroscope=()",
                "keyboard-map=()",
                "magnetometer=()",
                "microphone=()",
                "midi=()",
                "navigation-override=()",
                "payment=()",
                "picture-in-picture=()",
                "publickey-credentials-get=()",
                "screen-wake-lock=()",
                "sync-xhr=()",
                "usb=()",
                "web-share=()",
                "xr-spatial-tracking=()"
            };

            return string.Join(", ", policies);
        }

        private void RemoveServerHeaders(HttpResponse response)
        {
            // Удаляем информацию о сервере
            response.Headers.Remove("Server");
            response.Headers.Remove("X-Powered-By");
            response.Headers.Remove("X-AspNet-Version");
            response.Headers.Remove("X-AspNetMvc-Version");
        }

        private bool IsSensitiveEndpoint(PathString path)
        {
            var sensitiveEndpoints = new[]
            {
                "/api/auth/login",
                "/api/auth/register",
                "/api/auth/refresh",
                "/api/auth/profile",
                "/api/auth/change-password"
            };

            return sensitiveEndpoints.Any(endpoint =>
                path.StartsWithSegments(endpoint, StringComparison.OrdinalIgnoreCase));
        }
    }

    // Extension method для регистрации middleware
    public static class SecurityHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}