namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// Интерфейс для сервиса уведомления
    /// </summary>
    public interface INotificationService
    {
        public Task SendOrderCreateNotificationAsync(int orderId);
    }
}
