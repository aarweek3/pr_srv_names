using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;

namespace pr_srv_names.Middleware
{
    public class AuthExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public AuthExceptionMiddleware(RequestDelegate next, ILogger<AuthExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Обрабатываем 401 и 403 статусы для API endpoints, исключая тестовые пути
                await HandleHttpStatusCodes(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleHttpStatusCodes(HttpContext context)
        {
            if ((context.Response.StatusCode == 401 || context.Response.StatusCode == 403) &&
                !context.Response.HasStarted &&
                context.Request.Path.StartsWithSegments("/api") &&
                !context.Request.Path.StartsWithSegments("/api/test")) // Исключение для тестовых эндпоинтов
            {
                context.Response.ContentType = "application/json; charset=utf-8";

                var errorResponse = new ErrorResponseModel
                {
                    Success = false,
                    Message = context.Response.StatusCode == 401 ? "Unauthorized access" : "Access forbidden",
                    StatusCode = context.Response.StatusCode,
                    CorrelationId = context.TraceIdentifier,
                    ErrorCode = context.Response.StatusCode == 401 ? "UNAUTHORIZED" : "FORBIDDEN"
                };

                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = context.TraceIdentifier;

            // Логируем только серьезные ошибки как Error
            if (IsServerError(exception))
            {
                _logger.LogError(exception, "Server error occurred. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}",
                    correlationId, context.Request.Path, context.Request.Method);
            }
            else
            {
                _logger.LogWarning(exception, "Client error occurred. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}",
                    correlationId, context.Request.Path, context.Request.Method);
            }

            // Проверяем, что response еще не начался
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Cannot write error response - response has already started");
                return;
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            var errorResponse = CreateErrorResponse(exception, correlationId);
            context.Response.StatusCode = errorResponse.StatusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _env.IsDevelopment()
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
        }

        private static bool IsServerError(Exception exception)
        {
            return exception is not (ArgumentException or ArgumentNullException or
                                   UnauthorizedAccessException or SecurityTokenException or
                                   SecurityException or FileNotFoundException or
                                   DirectoryNotFoundException);
        }

        private ErrorResponseModel CreateErrorResponse(Exception exception, string correlationId)
        {
            return exception switch
            {
                SecurityTokenException or SecurityTokenValidationException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Invalid or expired token",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    CorrelationId = correlationId,
                    ErrorCode = "INVALID_TOKEN"
                },

                SecurityException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Security validation failed",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    CorrelationId = correlationId,
                    ErrorCode = "SECURITY_ERROR"
                },

                UnauthorizedAccessException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Unauthorized access",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    CorrelationId = correlationId,
                    ErrorCode = "UNAUTHORIZED"
                },

                ArgumentNullException argNull => new ErrorResponseModel
                {
                    Success = false,
                    Message = _env.IsDevelopment() ? $"Required parameter is missing: {argNull.ParamName}" : "Required parameter is missing",
                    StatusCode = StatusCodes.Status400BadRequest,
                    CorrelationId = correlationId,
                    ErrorCode = "MISSING_PARAMETER"
                },

                ArgumentException arg => new ErrorResponseModel
                {
                    Success = false,
                    Message = _env.IsDevelopment() ? arg.Message : "Invalid request parameters",
                    StatusCode = StatusCodes.Status400BadRequest,
                    CorrelationId = correlationId,
                    ErrorCode = "INVALID_PARAMETER"
                },

                FileNotFoundException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Requested file not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    CorrelationId = correlationId,
                    ErrorCode = "FILE_NOT_FOUND"
                },

                HttpRequestException httpEx => new ErrorResponseModel
                {
                    Success = false,
                    Message = _env.IsDevelopment() ? httpEx.Message : "External service error",
                    StatusCode = StatusCodes.Status502BadGateway,
                    CorrelationId = correlationId,
                    ErrorCode = "EXTERNAL_SERVICE_ERROR"
                },

                TimeoutException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Request timeout",
                    StatusCode = StatusCodes.Status408RequestTimeout,
                    CorrelationId = correlationId,
                    ErrorCode = "TIMEOUT"
                },

                TaskCanceledException => new ErrorResponseModel
                {
                    Success = false,
                    Message = "Request was cancelled",
                    StatusCode = StatusCodes.Status408RequestTimeout,
                    CorrelationId = correlationId,
                    ErrorCode = "REQUEST_CANCELLED"
                },

                _ => new ErrorResponseModel
                {
                    Success = false,
                    Message = _env.IsDevelopment() ? exception.Message : "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    CorrelationId = correlationId,
                    ErrorCode = "INTERNAL_ERROR"
                }
            };
        }
    }

    public class ErrorResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public static class AuthExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthExceptionMiddleware>();
        }
    }
}