using pr_srv_names.Models.Errors;

namespace pr_srv_names.Exceptions
{
    public class InvalidParametersException : BaseException
    {
        public InvalidParametersException(string errorKey = ErrorKeys.BadRequest)
            : base(errorKey, message: null)
        {
        }

        public InvalidParametersException(string errorKey, string customMessage)
            : base(errorKey, message: customMessage)
        {
        }
    }
}
