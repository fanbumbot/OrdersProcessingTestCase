using Riok.Mapperly.Abstractions;

namespace NotificationService.Notification
{
    /// <summary>
    /// Структура сообщения уведомления о выплате для WebSocket
    /// </summary>
    public record WebSocketPaymentNotification
    {
        public required int PaymentId { get; init; }
        public required bool Status { get; init; }
    }

    /// <summary>
    /// Структура сообщения уведомления о выплате  из Kafka
    /// </summary>
    public record KafkaPaymentNotifiaction
    {
        public required int PaymentId { get; init; }
        public required bool Status { get; init; }
    }
}
