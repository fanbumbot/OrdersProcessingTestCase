using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.Kafka;

namespace PaymentService.WebAPI.UseCases
{
    public sealed record UpdatePaymentStatusCommand(int paymentId, bool status) : IRequest;

    public class UpdatePaymentStatusHandler(AppDbContext context, INotificationService notificationService) : IRequestHandler<UpdatePaymentStatusCommand>
    {
        public async Task Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var model = await context.Payments.FindAsync(request.paymentId);
            if (model == null)
            {
                throw new PaymentNotFoundException(request.paymentId);
            }
            model.Status = request.status;
            await context.SaveChangesAsync();
            await notificationService.SendPaymentStatusUpdateNotificationAsync();
        }
    }
}
