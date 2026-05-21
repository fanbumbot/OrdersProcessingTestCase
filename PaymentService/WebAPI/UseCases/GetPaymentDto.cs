namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// DTO для получение информации о платеже
    /// </summary>
    public record GetPaymentDto
    {
        public required decimal Price { get; init; }
        public required bool Status { get; init; }
        public required DateTime DateCreate { get; init; }
    }
}
