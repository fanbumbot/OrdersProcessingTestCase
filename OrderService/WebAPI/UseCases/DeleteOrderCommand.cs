using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    public sealed record DeleteOrderCommand(int orderId) : IRequest;

    public class DeleteOrderHandler(AppDbContext context, OrderMapper mapper) : IRequestHandler<DeleteOrderCommand>
    {
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
