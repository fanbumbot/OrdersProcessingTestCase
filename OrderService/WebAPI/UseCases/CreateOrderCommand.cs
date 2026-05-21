using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;
using OrderService.WebAPI.Client;

namespace OrderService.WebAPI.UseCases
{
    public sealed record CreateOrderCommand(CreateOrderDto createDto) : IRequest<int>;

    public class CreateOrderHandler(
        AppDbContext context,
        OrderMapper mapper,
        IPaymentServiceClient paymentServiceClient) :
        IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            OrderModel model = mapper.MapCreateDtoToModel(request.createDto);
            await context.Orders.AddAsync(model);
            await context.SaveChangesAsync();

            var createPaymentDto = new CreatePaymentDto { OrderId = model.Id, Price = model.Price };
            await paymentServiceClient.CreatePaymentAsync(createPaymentDto);
            return model.Id;
        }
    }
}
