using PaymentService.WebAPI.UseCases;

using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task SendPaymentStatusUpdateNotificationAsync(int paymentId, bool status)
        {
            var notification = new KafkaNotification { paymentId = paymentId, status = status };
            await _kafkaProducer.PublishAsync("notifications", "PaymentStatusUpdate", notification);
        }
    }
}
