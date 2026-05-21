namespace PaymentService.WebAPI
{
    /// <summary>
    /// Исключение, что что-то не найдено
    /// </summary>
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message) { }
    }
}
