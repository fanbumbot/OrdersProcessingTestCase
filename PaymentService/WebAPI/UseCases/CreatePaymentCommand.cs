using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.WebAPI.UseCases
{
    public sealed record CreatePaymentCommand(CreatePaymentDto createDto) : IRequest<int>;

    public class CreatePaymentHandler(AppDbContext context, PaymentMapper mapper) : IRequestHandler<CreatePaymentCommand, int>
    {
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
