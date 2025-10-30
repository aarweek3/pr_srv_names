namespace pr_srv_names.Models.Errors
{
    public static partial class ErrorKeys
    {
        // Основные HTTP ошибки
        public const string ValidationError = "validation_error";
        public const string NotFound = "not_found";
        public const string BadRequest = "bad_request";
        public const string Unauthorized = "unauthorized";
        public const string Forbidden = "forbidden";
        public const string Conflict = "conflict";
        public const string InternalServerError = "internal_server_error";
        public const string ServiceUnavailable = "service_unavailable";
        public const string PreconditionFailed = "precondition_failed";

        // Специфичные ошибки
        public const string InvalidToken = "invalid_token";
        public const string TokenExpired = "token_expired";
        public const string CsrfTokenInvalid = "csrf_token_invalid";
        public const string DatabaseError = "database_error";
        public const string ExternalServiceError = "external_service_error";

        // Ограничения БД
        public const string UniqueConstraint = "unique_constraint_violation";
        public const string ForeignKeyViolation = "foreign_key_violation";
        public const string CheckConstraint = "check_constraint_violation";

        // ДОБАВЛЕНО: Доменные ошибки
        public const string CategoryNotFound = "category_not_found";
        public const string CategoryNameExists = "category_name_exists";
        public const string UserNotFound = "user_not_found";
        public const string EmailAlreadyExists = "email_already_exists";
        public const string AlbumNotFound = "album_not_found";
    }
}
