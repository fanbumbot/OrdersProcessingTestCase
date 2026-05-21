using Microsoft.AspNetCore.SignalR;

namespace NotificationService.WebAPI.Hubs
{
    /// <summary>
    /// Интерфейс для хаба Kafka для уведомлений
    /// </summary>
    public interface INotificationClient
    {
        Task SendPaymentStatusAsync(string message);
    }

    /// <summary>
    /// Хаб Kafka для уведомлений
    /// </summary>
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}
