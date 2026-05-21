

using OrderService.WebAPI.Client;
using Refit;

namespace OrderService.Tests.Unit.Mock
{
    public class FakePaymentServiceClient : IPaymentServiceClient
    {
        public Task CreatePaymentAsync([Body] CreatePaymentDto dto)
        {
            return Task.CompletedTask;
        }
    }
}
