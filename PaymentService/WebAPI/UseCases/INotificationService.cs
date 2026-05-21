namespace PaymentService.WebAPI.UseCases
{
    public interface INotificationService
    {
        public Task SendPaymentStatusUpdateNotificationAsync();
    }
}
