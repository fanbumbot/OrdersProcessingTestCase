using FluentValidation;

namespace PaymentService.WebAPI.UseCases
{
    public record PaymentCreateDto
    {
        public required int OrderId { get; init; }
        public required decimal Price { get; init; }
    }

    public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Необходимо указать ID заказа");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Необходимо указать цену")
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля");
        }
    }
}
