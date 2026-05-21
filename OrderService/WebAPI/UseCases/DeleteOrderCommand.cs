using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// Команда на удаление заказа по идентификатору
    /// </summary>
    /// <param name="orderId">Идентификатор</param>
    public sealed record DeleteOrderCommand(int orderId) : IRequest;

    /// <summary>
    /// Обработчик команды удаления заказа по идентификатору
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="mapper">Маппер</param>
    public class DeleteOrderHandler(AppDbContext context, OrderMapper mapper) : IRequestHandler<DeleteOrderCommand>
    {
        /// <summary>
        /// Обработчик команды
        /// </summary>
        /// <param name="request">Команда на удаление заказа</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>Ничего</returns>
        /// <exception cref="OrderNotFoundException">Заказ с таким идентификатором не найден</exception>
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var model = await context.Orders.FindAsync(request.orderId);
            if (model == null)
            {
                throw new OrderNotFoundException(request.orderId);
            }
            context.Orders.Remove(model);
            await context.SaveChangesAsync();
        }
    }
}
