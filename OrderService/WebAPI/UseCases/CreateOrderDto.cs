using FluentValidation;

namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// DTO для создания заказа
    /// </summary>
    public record CreateOrderDto
    {
        public required long ProductId { get; init; }
        public required int Amount { get; init; }
        public required string EmailClient { get; init; }
        public required decimal Price { get; init; }
        public required string PhoneNumber { get; init; }
    }

    /// <summary>
    /// Валидатор DTO для создания заказа
    /// </summary>
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Необходимо указать ID товара");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Необходимо указать количество товара")
                .GreaterThan(0).WithMessage("Количество товара должно быть больше нуля");

            RuleFor(x => x.EmailClient)
                .NotEmpty().WithMessage("Необходимо указать Email")
                .EmailAddress().WithMessage("Некорректный формат Email");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Необходимо указать цену")
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Необходимо указать номер телефона")
                .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Неверный формат номера телефона");
        }
    }
}
