namespace OrderService.WebAPI.UseCases
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int orderId) : 
            base($"Order with ID = {orderId} does not exist") { }
    }
}
