using FluentValidation;

namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// DTO для создания платежа
    /// </summary>
    public record CreatePaymentDto
    {
        public required int OrderId { get; init; }
        public required decimal Price { get; init; }
    }

    /// <summary>
    /// Валидатор DTO создания платежа
    /// </summary>
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Необходимо указать ID заказа");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Необходимо указать цену")
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля");
        }
    }
}
