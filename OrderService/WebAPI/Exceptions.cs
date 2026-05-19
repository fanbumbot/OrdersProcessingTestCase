namespace OrderService.WebAPI
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message) { }
    }

    public abstract class ValidationException : Exception
    {
        protected ValidationException(string message) : base(message) { }
    }
}
