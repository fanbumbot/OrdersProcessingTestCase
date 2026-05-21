using FluentValidation;

namespace OrderService.WebAPI.Client
{
    /// <summary>
    /// DTO для резерва средств
    /// </summary>
    public record CreatePaymentDto
    {
        public required int OrderId { get; init; }
        public required decimal Price { get; init; }
    }
}
