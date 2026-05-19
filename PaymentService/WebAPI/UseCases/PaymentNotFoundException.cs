namespace PaymentService.WebAPI.UseCases
{
    public class PaymentNotFoundException : NotFoundException
    {
        public PaymentNotFoundException(int paymentId) : 
            base($"Payment with ID = {paymentId} does not exist") { }
    }
}
