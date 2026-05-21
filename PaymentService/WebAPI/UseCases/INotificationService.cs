namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// Интерфейс для сервиса уведомления
    /// </summary>
    public interface INotificationService
    {
        public Task SendPaymentStatusUpdateNotificationAsync();
    }
}
