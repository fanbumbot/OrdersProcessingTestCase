using Riok.Mapperly.Abstractions;

namespace NotificationService.Notification
{
    [Mapper]
    public partial class NotificationMapper
    {
        public partial WebSocketPaymentNotification MapPayment(KafkaPaymentNotifiaction notification);
    }
}
