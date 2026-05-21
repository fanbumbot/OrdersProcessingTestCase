using PaymentService.WebAPI.UseCases;

namespace PaymentService.Kafka
{
    public class NotificationService : INotificationService
    {
        private readonly IKafkaProducer _kafkaProducer;
        public NotificationService(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async Task SendPaymentStatusUpdateNotificationAsync()
        {
            await _kafkaProducer.PublishAsync("notifications", "test", "hello");
        }
    }
}
