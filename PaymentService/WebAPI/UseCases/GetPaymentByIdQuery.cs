using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.WebAPI.UseCases
{
    public sealed record GetPaymentByIdQuery(int paymentId) : IRequest<GetPaymentDto>;

    public class GetPaymentByIdHandler(AppDbContext context, PaymentMapper mapper) : IRequestHandler<GetPaymentByIdQuery, GetPaymentDto>
    {
        public async Task<GetPaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await context.Payments.FindAsync(request.paymentId);
            if (model == null)
            {
                throw new PaymentNotFoundException(request.paymentId);
            }
            var result = mapper.MapModelToGetDto(model);
            return result;
        }
    }
}
