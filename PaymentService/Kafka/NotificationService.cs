using PaymentService.WebAPI.UseCases;

namespace PaymentService.Kafka
{
    /// <summary>
    /// Сервис (внешний) для оповещений
    /// Является портом для отправки сообщений через Kafka
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IKafkaProducer _kafkaProducer;
        public NotificationService(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// Отправление сообщения через Kafka
        /// </summary>
        public async Task SendPaymentStatusUpdateNotificationAsync()
        {
            await _kafkaProducer.PublishAsync("notifications", "test", "hello");
        }
    }
}
