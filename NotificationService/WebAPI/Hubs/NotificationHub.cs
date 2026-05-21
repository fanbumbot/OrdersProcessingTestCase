using Microsoft.AspNetCore.SignalR;

namespace NotificationService.WebAPI.Hubs
{
    public interface INotificationClient
    {
        Task ReceivePaymentStatusAsync(string message);
    }

    public class NotificationHub : Hub<INotificationClient>
    {

    }
}
