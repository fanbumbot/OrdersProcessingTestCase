using MediatR;
using OrderService.Kafka;

using OrderService.WebAPI.UseCases;

namespace PaymentService.Tests.Unit.Mock
{
    public class FakeNotificationService: INotificationService
    {
        public Task SendOrderCreateNotificationAsync(int orderId)
        {
            return Task.CompletedTask;
        }
    }
}
