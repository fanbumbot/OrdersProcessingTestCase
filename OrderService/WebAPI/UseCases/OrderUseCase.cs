namespace OrderService.WebAPI.UseCases
{
    public interface IOrderUseCase
    {
        public OrderReport GetAsync(int order_id);
    }

    public class OrderUseCase : IOrderUseCase
    {
        public OrderReport GetAsync(int order_id)
        {
            return new OrderReport();
        }
    }
}
