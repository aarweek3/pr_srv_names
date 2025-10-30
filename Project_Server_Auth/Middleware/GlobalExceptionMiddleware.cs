using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using pr_srv_names.Exceptions;
using pr_srv_names.Models.Errors;

namespace pr_srv_names.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        private static class PostgresErrorCodes
        {
            public const string ForeignKeyViolation = "23503";
            public const string UniqueViolation = "23505";
            public const string CheckViolation = "23514";
        }

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = context.TraceIdentifier;

            if (IsJwtException(exception))
            {
                await HandleJwtExceptionAsync(context, exception, correlationId);
                return;
            }

            var errorResponse = CreateErrorResponse(exception, correlationId);
            LogException(exception, correlationId, context);
            await WriteErrorResponse(context, errorResponse);
        }

        private static bool IsJwtException(Exception exception)
            => exception.GetType().Name.Contains("SecurityToken");

        private async Task HandleJwtExceptionAsync(HttpContext context, Exception exception, string correlationId)
        {
            _logger.LogWarning("JWT error: {Message}, CorrelationId: {CorrelationId}", exception.Message, correlationId);

            var messageKey = GetJwtErrorKey(exception);
            var errorResponse = ErrorResponse.Unauthorized(messageKey, correlationId);
            await WriteErrorResponse(context, errorResponse);
        }

        private static string GetJwtErrorKey(Exception exception)
        {
            return exception.GetType().Name switch
            {
                var name when name.Contains("Expired") => ErrorKeys.TokenExpired,
                _ => ErrorKeys.InvalidToken
            };
        }

        private async Task WriteErrorResponse(HttpContext context, ErrorResponse errorResponse)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = errorResponse.Status;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private ErrorResponse CreateErrorResponse(Exception exception, string correlationId)
        {
            exception = UnwrapAggregateException(exception);

            // ИСПРАВЛЕНО: Правильный порядок от СПЕЦИФИЧНЫХ к ОБЩИМ
            return exception switch
            {
                // 1. САМЫЕ СПЕЦИФИЧНЫЕ - ValidationException (FluentValidation)
                FluentValidation.ValidationException validationEx => ErrorResponse.Validation(
                    validationEx.Errors.Select(e => e.ErrorMessage), correlationId),

                // 2. СПЕЦИФИЧНЫЕ бизнес-исключения (наследники BaseException)
                ConflictException conflictEx => ErrorResponse.Conflict(
                    conflictEx.ErrorKey, correlationId, conflictEx.EntityName, conflictEx.ConflictField),

                NotFoundException notFoundEx => ErrorResponse.NotFound(
                    notFoundEx.EntityName ?? "Entity", correlationId),

                ForbiddenAccessException forbiddenEx => ErrorResponse.Forbidden(
                    forbiddenEx.ErrorKey, correlationId),

                InvalidParametersException invalidEx => ErrorResponse.BadRequest(
                    invalidEx.ErrorKey, correlationId),

                PreconditionFailedException preconditionEx => ErrorResponse.PreconditionFailed(
                    preconditionEx.ErrorKey, correlationId),

                // 3. БАЗОВЫЙ BaseException (для неучтенных наследников) - ПОСЛЕ специфичных
                BaseException baseEx => ErrorResponse.InternalServerError(
                    baseEx.ErrorKey, correlationId),

                // 4. Системные исключения
                AntiforgeryValidationException => ErrorResponse.Forbidden(ErrorKeys.CsrfTokenInvalid, correlationId),

                UnauthorizedAccessException => ErrorResponse.Unauthorized(ErrorKeys.Unauthorized, correlationId),

                HttpRequestException => ErrorResponse.ServiceUnavailable(ErrorKeys.ExternalServiceError, correlationId),

                DbUpdateException => ErrorResponse.InternalServerError(ErrorKeys.DatabaseError, correlationId),

                // 5. PostgreSQL исключения - обрабатываем через GetBaseException()
                var ex when ex.GetBaseException() is PostgresException pgEx => CreatePostgresErrorResponse(pgEx, correlationId),

                // 6. САМОЕ ОБЩЕЕ - все остальные исключения
                _ => ErrorResponse.InternalServerError(
                    _env.IsDevelopment() ? exception.Message : ErrorKeys.InternalServerError,
                    correlationId)
            };
        }

        private Exception UnwrapAggregateException(Exception exception)
        {
            while (exception is AggregateException aggEx)
            {
                // УЛУЧШЕНО: Приоритет специфичным исключениям
                exception = aggEx.Flatten().InnerExceptions.FirstOrDefault(e =>
                    e is FluentValidation.ValidationException ||
                    e is ConflictException ||
                    e is NotFoundException ||
                    e is ForbiddenAccessException ||
                    e is InvalidParametersException ||
                    e is PreconditionFailedException)
                    ?? aggEx.Flatten().InnerExceptions.FirstOrDefault(e => e is BaseException)
                    ?? aggEx.Flatten().InnerExceptions.First();
            }
            return exception;
        }

        private ErrorResponse CreatePostgresErrorResponse(PostgresException pgEx, string correlationId)
        {
            return pgEx.SqlState switch
            {
                PostgresErrorCodes.ForeignKeyViolation => ErrorResponse.ForeignKeyViolation(correlationId),
                PostgresErrorCodes.UniqueViolation => CreateUniqueViolationError(pgEx, correlationId),
                PostgresErrorCodes.CheckViolation => ErrorResponse.BadRequest(ErrorKeys.CheckConstraint, correlationId),
                _ => ErrorResponse.InternalServerError(
                    _env.IsDevelopment() ? pgEx.Message : ErrorKeys.DatabaseError, correlationId)
            };
        }

        private static ErrorResponse CreateUniqueViolationError(PostgresException pgEx, string correlationId)
        {
            var field = pgEx.ConstraintName?.ToLower() switch
            {
                var name when name.Contains("email") => "Email",
                var name when name.Contains("username") => "Username",
                var name when name.Contains("url_slug") => "UrlSlug",
                var name when name.Contains("name") => "Name", // Добавлено для Category
                _ => "UnknownField"
            };

            return ErrorResponse.Conflict(ErrorKeys.UniqueConstraint, correlationId, null, field);
        }

        private void LogException(Exception exception, string correlationId, HttpContext context)
        {
            var logData = new
            {
                Method = context.Request.Method,
                Path = context.Request.Path.Value,
                User = context.User?.Identity?.Name ?? "Anonymous",
                CorrelationId = correlationId
            };

            // УЛУЧШЕНО: Правильный порядок логирования тоже важен
            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    _logger.LogWarning("Validation failed: {Errors} {@LogData}",
                        string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)), logData);
                    break;

                case ConflictException conflictEx:
                    _logger.LogWarning("Conflict: {ErrorKey} - {Message} {@LogData}",
                        conflictEx.ErrorKey, conflictEx.Message, logData);
                    break;

                case NotFoundException notFoundEx:
                    _logger.LogWarning("Not found: {ErrorKey} - {Message} {@LogData}",
                        notFoundEx.ErrorKey, notFoundEx.Message, logData);
                    break;

                case ForbiddenAccessException forbiddenEx:
                    _logger.LogWarning("Forbidden: {ErrorKey} - {Message} {@LogData}",
                        forbiddenEx.ErrorKey, forbiddenEx.Message, logData);
                    break;

                case InvalidParametersException invalidEx:
                    _logger.LogWarning("Invalid parameters: {ErrorKey} - {Message} {@LogData}",
                        invalidEx.ErrorKey, invalidEx.Message, logData);
                    break;

                case PreconditionFailedException preconditionEx:
                    _logger.LogWarning("Precondition failed: {ErrorKey} - {Message} {@LogData}",
                        preconditionEx.ErrorKey, preconditionEx.Message, logData);
                    break;

                case BaseException baseEx:
                    _logger.LogWarning("Domain exception: {ErrorKey} - {Message} {@LogData}",
                        baseEx.ErrorKey, baseEx.Message, logData);
                    break;

                case UnauthorizedAccessException:
                    _logger.LogWarning("Unauthorized access attempt {@LogData}", logData);
                    break;

                case var ex when ex.GetBaseException() is PostgresException pgEx:
                    _logger.LogError("Database error: {SqlState} - {Message} {@LogData}",
                        pgEx.SqlState, pgEx.Message, logData);
                    break;

                default:
                    _logger.LogError(exception, "Request processing error {@LogData}", logData);
                    break;
            }
        }
    }
}