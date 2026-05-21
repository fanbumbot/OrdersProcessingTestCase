namespace OrderService.WebAPI
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message) { }
    }

    public class ValidationExceptionr : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationExceptionr(IDictionary<string, string[]> errors) :
            base("One or more parameters does not pass validation")
        {
            Errors = errors;
        }
    }
}
