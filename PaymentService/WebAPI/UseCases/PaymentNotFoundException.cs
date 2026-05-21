namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// Исключение, которое говорит о том, что платёж не найдн
    /// </summary>
    public class PaymentNotFoundException : NotFoundException
    {
        public PaymentNotFoundException(int paymentId) : 
            base($"Payment with ID = {paymentId} does not exist") { }
    }
}
