namespace PaymentService.WebAPI.UseCases
{
    public record GetPaymentDto
    {
        public required decimal Price { get; init; }
        public required bool Status { get; init; }
        public required DateTime DateCreate { get; init; }
    }
}
