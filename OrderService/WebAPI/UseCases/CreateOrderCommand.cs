using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;
using OrderService.WebAPI.Client;

namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// Команда создания заказа
    /// </summary>
    /// <param name="createDto"></param>
    public sealed record CreateOrderCommand(CreateOrderDto createDto) : IRequest<int>;

    /// <summary>
    /// Валидатор команды создания заказа
    /// </summary>
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator(IValidator<CreateOrderDto> dtoValidator)
        {
            RuleFor(x => x.createDto).SetValidator(dtoValidator);
        }
    }

    /// <summary>
    /// Обработчик команды создания заказа
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="mapper">Маппер</param>
    /// <param name="paymentServiceClient">Клиент сервиса оплаты заказов</param>
    public class CreateOrderHandler(
        AppDbContext context,
        OrderMapper mapper,
        IPaymentServiceClient paymentServiceClient,
        INotificationService notificationService
    ) :
        IRequestHandler<CreateOrderCommand, int>
    {
        /// <summary>
        /// Команда обработчик команды создания заказа
        /// </summary>
        /// <param name="request">Команда на создание заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>Идентификатор заказа</returns>
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            OrderModel model = mapper.MapCreateDtoToModel(request.createDto);
            await context.Orders.AddAsync(model);
            await context.SaveChangesAsync();

            var createPaymentDto = new CreatePaymentDto { OrderId = model.Id, Price = model.Price };
            await paymentServiceClient.CreatePaymentAsync(createPaymentDto);
            await notificationService.SendOrderCreateNotificationAsync(model.Id);
            return model.Id;
        }
    }
}
