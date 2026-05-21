using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    public sealed record GetOrderByIdQuery(int orderId) : IRequest<GetOrderDto>;

    public class GetOrderByIdHandler(AppDbContext context, OrderMapper mapper) : IRequestHandler<GetOrderByIdQuery, GetOrderDto>
    {
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
