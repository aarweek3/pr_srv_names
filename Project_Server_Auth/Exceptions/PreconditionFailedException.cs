using pr_srv_names.Models.Errors;

namespace pr_srv_names.Exceptions
{
    public class PreconditionFailedException : BaseException
    {
        public PreconditionFailedException(string errorKey = ErrorKeys.PreconditionFailed)
            : base(errorKey, message: null)
        {
        }

        public PreconditionFailedException(string errorKey, string customMessage)
            : base(errorKey, message: customMessage)
        {
        }
    }
}
