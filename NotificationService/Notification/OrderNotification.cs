using Riok.Mapperly.Abstractions;

namespace NotificationService.Notification
{
    /// <summary>
    /// Структура сообщения уведомления о создании заказа для WebSocket
    /// </summary>
    public record WebSocketOrderNotification
    {
        public required int OrderId { get; init; }
    }

    /// <summary>
    /// Структура сообщения уведомления о создании заказа из Kafka
    /// </summary>
    public record KafkaOrderNotifiaction
    {
        public required int OrderId { get; init; }
    }
}
