using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    /// <summary>
    /// Запрос на получение информации о заказе по идентификатору
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    public sealed record GetOrderByIdQuery(int orderId) : IRequest<GetOrderDto>;

    /// <summary>
    /// Обработчик команды на получение информации о заказе
    /// </summary>
    /// <param name="context">Контекст о базе данных</param>
    /// <param name="mapper">Маппер</param>
    public class GetOrderByIdHandler(AppDbContext context, OrderMapper mapper) : IRequestHandler<GetOrderByIdQuery, GetOrderDto>
    {
        /// <summary>
        /// Обработчик команды
        /// </summary>
        /// <param name="request">Запрос на получение данных</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>Информация о заказе в виде DTO</returns>
        /// <exception cref="OrderNotFoundException">Зказа с таким идентификатором не найден</exception>
        public async Task<GetOrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await context.Orders.FindAsync(request.orderId);
            if (model == null)
            {
                throw new OrderNotFoundException(request.orderId);
            }
            var result = mapper.MapModelToGetDto(model);
            return result;
        }
    }
}
