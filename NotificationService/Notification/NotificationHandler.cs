using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Notification.WebSocketHubs;
using System.Text.Json;

namespace NotificationService.Notification
{
    public interface INotificationHandler
    {
        public Task HandleRawNotificationAsync(Message<string, string> message);
    }

    public class NotificationHandler : INotificationHandler
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly NotificationMapper _mapper;

        public NotificationHandler(
            IHubContext<NotificationHub, INotificationClient> hubContext,
            NotificationMapper mapper
        )
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task HandleRawNotificationAsync(Message<string, string> message)
        {
            var key = message.Key;
            var rawJson = message.Value;

            if (key == "PaymentStatusUpdate")
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var notification = JsonSerializer.Deserialize<KafkaPaymentNotifiaction>(rawJson, options);

                var paymentNotification = _mapper.MapPayment(notification);
                await _hubContext.Clients.All.SendPaymentStatusAsync(paymentNotification);
            }
        }
    }
}
