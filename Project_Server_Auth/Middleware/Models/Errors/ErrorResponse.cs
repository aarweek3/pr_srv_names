namespace pr_srv_names.Models.Errors
{
    public class ErrorResponse
    {
        public string Type { get; set; } = "https://tools.ietf.org/html/rfc7807";
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public List<string>? Details { get; set; }
        public string? Instance { get; set; }
        public string? EntityName { get; set; }
        public string? ConflictField { get; set; }
        public object? AdditionalData { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Основные методы создания ошибок
        public static ErrorResponse Create(int statusCode, string messageKey, string correlationId, params object[] args)
        {
            return new ErrorResponse
            {
                Type = GetTypeFromStatus(statusCode),
                Title = GetTitleFromStatus(statusCode),
                Status = statusCode,
                Detail = ErrorMessages.GetMessage(messageKey, args),
                Instance = correlationId,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse Validation(IEnumerable<string> errors, string correlationId)
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation Error",
                Status = 400,
                Detail = ErrorMessages.GetMessage(ErrorKeys.ValidationError),
                Details = errors.ToList(),
                Instance = correlationId,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse NotFound(string entityName, string correlationId, string path = null)
        {
            var detail = path != null
                ? $"{entityName} по пути '{path}' не найден"
                : $"{entityName} не найден";

            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Not Found",
                Status = 404,
                Detail = detail,
                Instance = correlationId,
                EntityName = entityName,
                AdditionalData = path != null ? new { path } : null,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse BadRequest(string messageKey, string correlationId, object additionalData = null)
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Bad Request",
                Status = 400,
                Detail = ErrorMessages.GetMessage(messageKey),
                Instance = correlationId,
                AdditionalData = additionalData,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse Unauthorized(string messageKey, string correlationId)
            => Create(401, messageKey, correlationId);

        public static ErrorResponse Forbidden(string messageKey, string correlationId, object additionalData = null)
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Title = "Forbidden",
                Status = 403,
                Detail = ErrorMessages.GetMessage(messageKey),
                Instance = correlationId,
                AdditionalData = additionalData,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse Conflict(string messageKey, string correlationId, string entityName = null, string field = null)
        {
            var response = Create(409, messageKey, correlationId);
            if (entityName != null) response.EntityName = entityName;
            if (field != null) response.ConflictField = field;
            return response;
        }

        public static ErrorResponse InternalServerError(string messageKey, string correlationId, object additionalData = null)
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = 500,
                Detail = ErrorMessages.GetMessage(messageKey),
                Instance = correlationId,
                AdditionalData = additionalData,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ErrorResponse ServiceUnavailable(string messageKey, string correlationId)
            => Create(503, messageKey, correlationId);

        public static ErrorResponse PreconditionFailed(string messageKey, string correlationId)
            => Create(412, messageKey, correlationId);

        /// <summary>
        /// Создает ответ для ошибки недостаточного места на диске (507 Insufficient Storage)
        /// </summary>
        public static ErrorResponse InsufficientStorage(string messageKey, string correlationId, object additionalData = null)
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.8",
                Title = "Insufficient Storage",
                Status = 507,
                Detail = ErrorMessages.GetMessage(messageKey),
                Instance = correlationId,
                AdditionalData = additionalData,
                Timestamp = DateTime.UtcNow
            };
        }

        // Специфичные методы для БД ошибок
        public static ErrorResponse ForeignKeyViolation(string correlationId)
            => Create(409, ErrorKeys.ForeignKeyViolation, correlationId);

        // Extension методы для цепочечных вызовов
        public ErrorResponse WithEntityName(string entityName)
        {
            EntityName = entityName;
            return this;
        }

        public ErrorResponse WithConflictField(string field)
        {
            ConflictField = field;
            return this;
        }

        public ErrorResponse WithAdditionalData(object data)
        {
            AdditionalData = data;
            return this;
        }

        private static string GetTypeFromStatus(int statusCode) => statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
            403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            412 => "https://tools.ietf.org/html/rfc7232#section-4.2",
            429 => "https://tools.ietf.org/html/rfc6585#section-4",
            500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            503 => "https://tools.ietf.org/html/rfc7231#section-6.6.4",
            507 => "https://tools.ietf.org/html/rfc7231#section-6.6.8",
            _ => "https://tools.ietf.org/html/rfc7807"
        };

        private static string GetTitleFromStatus(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            412 => "Precondition Failed",
            429 => "Too Many Requests",
            500 => "Internal Server Error",
            503 => "Service Unavailable",
            507 => "Insufficient Storage",
            _ => "Error"
        };
    }
}