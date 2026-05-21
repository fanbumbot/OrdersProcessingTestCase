namespace OrderService.WebAPI.UseCases
{
    public record GetOrderDto
    {
        public required long ProductId { get; init; }
        public required int Amount { get; init; }
        public required string EmailClient { get; init; }
        public required decimal Price { get; init; }
        public required string PhoneNumber { get; init; }
    }
}
