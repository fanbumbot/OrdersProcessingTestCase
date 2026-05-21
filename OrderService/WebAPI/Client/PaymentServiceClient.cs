using Refit;

namespace OrderService.WebAPI.Client
{
    public interface IPaymentServiceClient
    {
        [Post("/payment/create")]
        Task CreatePaymentAsync([Body] CreatePaymentDto dto);
    }
}
