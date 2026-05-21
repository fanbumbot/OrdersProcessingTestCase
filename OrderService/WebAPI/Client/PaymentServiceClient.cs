using Refit;

namespace OrderService.WebAPI.Client
{
    /// <summary>
    /// Клиент для отправки запроса на резерв средств при создании заказа
    /// </summary>
    public interface IPaymentServiceClient
    {
        [Post("/payment/create")]
        Task CreatePaymentAsync([Body] CreatePaymentDto dto);
    }
}
