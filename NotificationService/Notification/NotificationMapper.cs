using Riok.Mapperly.Abstractions;

namespace NotificationService.Notification
{
    /// <summary>
    /// Маппер для уведомлений
    /// </summary>
    [Mapper]
    public partial class NotificationMapper
    {
        public partial WebSocketPaymentNotification MapPayment(KafkaPaymentNotifiaction notification);
    }
}
