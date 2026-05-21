using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// Команда для создания данных о платеже о заказе
    /// </summary>
    /// <param name="createDto">DTO для создания</param>
    public sealed record CreatePaymentCommand(CreatePaymentDto createDto) : IRequest<int>;

    /// <summary>
    /// Валидатор данных о платеже
    /// </summary>
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator(IValidator<CreatePaymentDto> dtoValidator)
        {
            RuleFor(x => x.createDto).SetValidator(dtoValidator);
        }
    }

    /// <summary>
    /// Обработчик команды для создания данных о платеже
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="mapper">Маппер</param>
    public class CreatePaymentHandler(AppDbContext context, PaymentMapper mapper) : IRequestHandler<CreatePaymentCommand, int>
    {
        /// <summary>
        /// Обработчик команды
        /// </summary>
        /// <param name="request">Команда для создания данных о платеже</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>Идентификатор данных о платеже</returns>
        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var timestamp = DateTime.UtcNow;
            PaymentModel model = mapper.MapCreateDtoToModel(request.createDto, timestamp);
            await context.Payments.AddAsync(model);
            await context.SaveChangesAsync();
            var modelId = model.Id;
            return modelId;
        }
    }
}
