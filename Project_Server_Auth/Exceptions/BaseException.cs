using pr_srv_names.Models.Errors;

namespace pr_srv_names.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string ErrorKey { get; }
        public string? EntityName { get; }

        protected BaseException(
            string errorKey,
            string? message = null,
            string? entityName = null,
            Exception? innerException = null)
            : base(message ?? ErrorMessages.GetMessage(errorKey), innerException)
        {
            ErrorKey = errorKey;
            EntityName = entityName;
        }
    }
}