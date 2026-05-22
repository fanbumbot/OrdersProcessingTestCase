using Riok.Mapperly.Abstractions;

namespace NotificationService.Notification
{
    public record WebSocketPaymentNotification
    {
        public required int PaymentId { get; init; }
        public required bool Status { get; init; }
    }

    public record KafkaPaymentNotifiaction
    {
        public required int PaymentId { get; init; }
        public required bool Status { get; init; }
    }
}
