using pr_srv_names.Models.Errors;

namespace pr_srv_names.Exceptions
{
    public class ForbiddenAccessException : BaseException
    {
        public ForbiddenAccessException(string errorKey = ErrorKeys.Forbidden)
            : base(errorKey, message: null)
        {
        }

        public ForbiddenAccessException(string errorKey, string customMessage)
            : base(errorKey, message: customMessage)
        {
        }
    }
}
