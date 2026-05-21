using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.WebAPI.UseCases
{
    public sealed record CreateOrderCommand(CreateOrderDto createDto) : IRequest<int>;

    public class CreateOrderHandler(AppDbContext context, OrderMapper mapper) : IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            OrderModel model = mapper.MapCreateDtoToModel(request.createDto);
            await context.Orders.AddAsync(model);
            await context.SaveChangesAsync();
            var modelId = model.Id;
            return modelId;
        }
    }
}
