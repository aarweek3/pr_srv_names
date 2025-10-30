// Middleware/RateLimitingMiddleware.cs
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace pr_srv_names.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        // Настройки rate limiting
        private readonly Dictionary<string, RateLimitConfig> _rateLimitConfigs = new()
        {
            { "/api/auth/login", new RateLimitConfig { MaxRequests = 5, TimeWindow = TimeSpan.FromMinutes(1) } },
            { "/api/auth/register", new RateLimitConfig { MaxRequests = 3, TimeWindow = TimeSpan.FromMinutes(5) } },
            { "/api/auth/refresh", new RateLimitConfig { MaxRequests = 10, TimeWindow = TimeSpan.FromMinutes(1) } }
        };

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = GetEndpointPath(context.Request.Path);

            if (_rateLimitConfigs.TryGetValue(endpoint, out var config))
            {
                var clientId = GetClientIdentifier(context);
                var isAllowed = await CheckRateLimitAsync(clientId, endpoint, config);

                if (!isAllowed)
                {
                    _logger.LogWarning("Rate limit exceeded for client {ClientId} on endpoint {Endpoint}",
                        clientId, endpoint);

                    await HandleRateLimitExceeded(context, config);
                    return;
                }
            }

            await _next(context);
        }

        private string GetEndpointPath(PathString path)
        {
            return path.Value?.ToLower() ?? "";
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Комбинируем IP и User-Agent для более точной идентификации
            var ipAddress = GetClientIpAddress(context);
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var userAgentHash = userAgent.GetHashCode().ToString();

            return $"{ipAddress}:{userAgentHash}";
        }

        private string GetClientIpAddress(HttpContext context)
        {
            // Проверяем заголовки прокси
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var firstIp = forwardedFor.Split(',')[0].Trim();
                if (IPAddress.TryParse(firstIp, out _))
                    return firstIp;
            }

            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp) && IPAddress.TryParse(realIp, out _))
                return realIp;

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private async Task<bool> CheckRateLimitAsync(string clientId, string endpoint, RateLimitConfig config)
        {
            var key = $"rate_limit:{endpoint}:{clientId}";

            if (_cache.TryGetValue(key, out RateLimitInfo? info))
            {
                if (info.WindowStart.Add(config.TimeWindow) <= DateTime.UtcNow)
                {
                    // Окно истекло, сбрасываем счетчик
                    info.RequestCount = 1;
                    info.WindowStart = DateTime.UtcNow;
                }
                else
                {
                    info.RequestCount++;
                }

                if (info.RequestCount > config.MaxRequests)
                {
                    // Увеличиваем время блокировки при превышении лимита
                    var penaltyTime = CalculatePenaltyTime(info.ViolationCount);
                    info.ViolationCount++;

                    _cache.Set(key, info, penaltyTime);
                    return false;
                }
            }
            else
            {
                info = new RateLimitInfo
                {
                    RequestCount = 1,
                    WindowStart = DateTime.UtcNow,
                    ViolationCount = 0
                };
            }

            _cache.Set(key, info, config.TimeWindow);
            return true;
        }

        private TimeSpan CalculatePenaltyTime(int violationCount)
        {
            // Прогрессивные штрафы
            return violationCount switch
            {
                0 => TimeSpan.FromMinutes(1),
                1 => TimeSpan.FromMinutes(5),
                2 => TimeSpan.FromMinutes(15),
                3 => TimeSpan.FromHours(1),
                _ => TimeSpan.FromHours(24)
            };
        }

        private async Task HandleRateLimitExceeded(HttpContext context, RateLimitConfig config)
        {
            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.Headers.Add("Retry-After", config.TimeWindow.TotalSeconds.ToString());
            context.Response.Headers.Add("X-RateLimit-Limit", config.MaxRequests.ToString());
            context.Response.Headers.Add("X-RateLimit-Window", config.TimeWindow.TotalSeconds.ToString());

            var response = new
            {
                error = "rate_limit_exceeded",
                message = "Превышен лимит запросов. Попробуйте позже.",
                retryAfter = config.TimeWindow.TotalSeconds
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }

    public class RateLimitConfig
    {
        public int MaxRequests { get; set; }
        public TimeSpan TimeWindow { get; set; }
    }

    public class RateLimitInfo
    {
        public int RequestCount { get; set; }
        public DateTime WindowStart { get; set; }
        public int ViolationCount { get; set; }
    }
}