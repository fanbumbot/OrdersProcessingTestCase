using FluentValidation;

namespace OrderService.WebAPI.Client
{
    public record CreatePaymentDto
    {
        public required int OrderId { get; init; }
        public required decimal Price { get; init; }
    }
}
