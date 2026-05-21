namespace OrderService.WebAPI
{
    /// <summary>
    /// Исключение о том, что не удалось найти что-то
    /// </summary>
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message) { }
    }
}
