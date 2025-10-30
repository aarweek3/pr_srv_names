using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки 401 ошибки
        /// </summary>
        [HttpGet("test-401")]
        public IActionResult Test401()
        {
            _logger.LogInformation("Test 401 endpoint called");

            return Unauthorized(new
            {
                success = false,
                message = "Сессия истекла. Пожалуйста, войдите заново",
                data = (object?)null,
                statusCode = 401,
                errorCode = "UNAUTHORIZED_TEST",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки 403 ошибки
        /// </summary>
        [HttpGet("test-403")]
        public IActionResult Test403()
        {
            _logger.LogInformation("Test 403 endpoint called");

            return StatusCode(403, new
            {
                success = false,
                message = "Access forbidden",
                data = (object?)null,
                statusCode = 403,
                errorCode = "FORBIDDEN_TEST",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Альтернативный тестовый эндпоинт для 403 с кастомным ответом
        /// </summary>
        [HttpGet("test-403-custom")]
        public IActionResult Test403Custom()
        {
            _logger.LogInformation("Test 403 custom endpoint called");

            return StatusCode(403, new
            {
                success = false,
                message = "Access forbidden - custom",
                data = (object?)null,
                statusCode = 403,
                errorCode = "FORBIDDEN_CUSTOM_TEST",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки заголовков безопасности
        /// </summary>
        [HttpGet("test-headers")]
        public IActionResult TestHeaders()
        {
            _logger.LogInformation("Test headers endpoint called");

            return Ok(new
            {
                success = true,
                message = "Headers test successful",
                data = new
                {
                    timestamp = DateTime.UtcNow,
                    headers = Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    serverTime = DateTimeOffset.UtcNow.ToString("O")
                }
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки исключений
        /// </summary>
        [HttpGet("test-exception")]
        public IActionResult TestException()
        {
            _logger.LogInformation("Test exception endpoint called");

            throw new InvalidOperationException("Тестовое исключение для проверки обработки ошибок");
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки таймаута
        /// </summary>
        [HttpGet("test-timeout")]
        public async Task<IActionResult> TestTimeout(int delay = 5000)
        {
            _logger.LogInformation("Test timeout endpoint called with delay: {Delay}ms", delay);

            if (delay > 30000)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Delay too long (max 30 seconds)",
                    statusCode = 400,
                    timestamp = DateTime.UtcNow
                });
            }

            await Task.Delay(delay);

            return Ok(new
            {
                success = true,
                message = $"Delayed response after {delay}ms",
                data = new { delayMs = delay },
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки валидации токена
        /// </summary>
        [HttpGet("test-token-validation")]
        [Authorize]
        public IActionResult TestTokenValidation()
        {
            _logger.LogInformation("Test token validation endpoint called");

            // Ищем claims по всем возможным типам
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value
                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            if (roles.Length == 0)
            {
                roles = User.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    .Select(c => c.Value)
                    .ToArray();
            }

            // Группируем все claims
            var userClaims = User.Claims
                .GroupBy(c => c.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count() > 1 ? string.Join(", ", g.Select(c => c.Value)) : g.First().Value
                );

            return Ok(new
            {
                success = true,
                message = "Token validation successful",
                data = new
                {
                    userId = userId,
                    email = email,
                    roles = roles.Distinct().ToArray(),
                    claims = userClaims,
                    isAuthenticated = User.Identity?.IsAuthenticated ?? false
                },
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки CORS (GET)
        /// </summary>
        [HttpGet("test-cors")]
        public IActionResult TestCors()
        {
            _logger.LogInformation("Test CORS GET endpoint called with method: {Method}", Request.Method);

            return Ok(new
            {
                success = true,
                message = "CORS test successful",
                data = new
                {
                    method = Request.Method,
                    corsHeaders = new
                    {
                        origin = Response.Headers["Access-Control-Allow-Origin"].ToString(),
                        methods = Response.Headers["Access-Control-Allow-Methods"].ToString(),
                        headers = Response.Headers["Access-Control-Allow-Headers"].ToString(),
                        credentials = Response.Headers["Access-Control-Allow-Credentials"].ToString()
                    }
                },
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Тестовый эндпоинт для проверки различных HTTP методов
        /// </summary>
        [HttpPost("test-methods")]
        [HttpPut("test-methods")]
        [HttpPatch("test-methods")]
        [HttpDelete("test-methods")]
        public IActionResult TestMethods()
        {
            _logger.LogInformation("Test methods endpoint called with method: {Method}", Request.Method);

            return Ok(new
            {
                success = true,
                message = $"Method {Request.Method} successful",
                data = new
                {
                    method = Request.Method,
                    path = Request.Path.Value,
                    timestamp = DateTime.UtcNow
                }
            });
        }

        /// <summary>
        /// Простой эндпоинт для проверки работоспособности
        /// </summary>
        [HttpGet("test-ping")]
        public IActionResult TestPing()
        {
            _logger.LogInformation("Test ping endpoint called");

            return Ok(new
            {
                success = true,
                message = "Pong",
                data = new
                {
                    timestamp = DateTime.UtcNow,
                    server = "Project_Server_Auth",
                    version = "1.0.0",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                }
            });
        }
    }
}