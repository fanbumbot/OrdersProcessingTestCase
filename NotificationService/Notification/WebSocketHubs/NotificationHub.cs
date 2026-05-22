using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Notification.WebSocketHubs
{

    /// <summary>
    /// Интерфейс для хаба Kafka для уведомлений
    /// </summary>
    public interface INotificationClient
    {
        Task SendPaymentStatusAsync(WebSocketPaymentNotification notification);
    }

    /// <summary>
    /// Хаб Kafka для уведомлений
    /// </summary>
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}
