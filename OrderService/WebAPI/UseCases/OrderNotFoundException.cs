namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// Исключение о том, что система не нашла заказ по идентификатору
    /// </summary>
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int orderId) : 
            base($"Order with ID = {orderId} does not exist") { }
    }
}
