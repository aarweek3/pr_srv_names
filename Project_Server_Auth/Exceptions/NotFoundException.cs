namespace pr_srv_names.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string errorKey, string? entityName = null)
            : base(errorKey, message: null, entityName: entityName)
        {
        }

        public NotFoundException(string errorKey, string customMessage, string? entityName = null)
            : base(errorKey, message: customMessage, entityName: entityName)
        {
        }
    }
}
