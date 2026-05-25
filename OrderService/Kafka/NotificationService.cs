using OrderService.WebAPI.UseCases;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderService.Kafka
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
        public async Task SendOrderCreateNotificationAsync(int orderId)
        {
            var notification = new KafkaOrderNotification { orderId = orderId };
            await _kafkaProducer.PublishAsync("notifications", "OrderCreate", notification);
        }
    }
}
