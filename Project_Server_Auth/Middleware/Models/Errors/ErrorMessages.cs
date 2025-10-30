namespace pr_srv_names.Models.Errors
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<string, string> _messages = new()
        {
            // Основные HTTP ошибки
            { ErrorKeys.ValidationError, "Ошибка валидации данных" },
            { ErrorKeys.NotFound, "Ресурс не найден" },
            { ErrorKeys.BadRequest, "Неверный запрос" },
            { ErrorKeys.Unauthorized, "Неавторизованный доступ" },
            { ErrorKeys.Forbidden, "Доступ запрещен" },
            { ErrorKeys.Conflict, "Конфликт данных" },
            { ErrorKeys.InternalServerError, "Внутренняя ошибка сервера" },
            { ErrorKeys.ServiceUnavailable, "Сервис недоступен" },
            { ErrorKeys.PreconditionFailed, "Предварительное условие не выполнено" }, // ДОБАВЛЕНО
            
            // Специфичные ошибки
            { ErrorKeys.InvalidToken, "Недействительный токен" },
            { ErrorKeys.TokenExpired, "Токен истек" },
            { ErrorKeys.CsrfTokenInvalid, "CSRF токен недействителен" },
            { ErrorKeys.DatabaseError, "Ошибка базы данных" },
            { ErrorKeys.ExternalServiceError, "Ошибка внешнего сервиса" },
            
            // Ограничения БД
            { ErrorKeys.UniqueConstraint, "Нарушение уникальности" },
            { ErrorKeys.ForeignKeyViolation, "Нельзя удалить - есть связанные данные" },
            { ErrorKeys.CheckConstraint, "Нарушение ограничения базы данных" },

            // Доменные сообщения
            { ErrorKeys.CategoryNotFound, "Категория не найдена" },
            { ErrorKeys.CategoryNameExists, "Категория с таким названием уже существует" },
            { ErrorKeys.UserNotFound, "Пользователь не найден" },
            { ErrorKeys.EmailAlreadyExists, "Пользователь с таким email уже существует" },
            { ErrorKeys.AlbumNotFound, "Альбом не найден" }
        };

        public static string GetMessage(string key, params object[] args)
        {
            if (_messages.TryGetValue(key, out var message))
            {
                return args.Length > 0 ? string.Format(message, args) : message;
            }
            return key; // Возвращаем ключ как fallback
        }
    }
}
