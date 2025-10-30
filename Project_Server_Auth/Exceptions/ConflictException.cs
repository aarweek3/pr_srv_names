namespace pr_srv_names.Exceptions
{
    public class ConflictException : BaseException
    {
        public string? ConflictField { get; }

        // ѕервый конструктор: автоматическое сообщение
        public ConflictException(string errorKey, string? entityName = null, string? conflictField = null)
            : base(errorKey, message: null, entityName: entityName)
        {
            ConflictField = conflictField;
        }

        // ¬торой конструктор: кастомное сообщение
        public ConflictException(string errorKey, string customMessage, string? entityName = null, string? conflictField = null)
            : base(errorKey, message: customMessage, entityName: entityName)
        {
            ConflictField = conflictField;
        }
    }
}