using MediatR;
using PaymentService.Kafka;

using PaymentService.WebAPI.UseCases;

namespace PaymentService.Tests.Unit.Mock
{
    public class FakeNotificationService: INotificationService
    {
        public Task SendPaymentStatusUpdateNotificationAsync()
        {
            return Task.CompletedTask;
        }
    }
}
