using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.Kafka;

namespace PaymentService.WebAPI.UseCases
{
    /// <summary>
    /// Команда обновления статуса платежа
    /// </summary>
    /// <param name="paymentId">Идентификатор платежа</param>
    /// <param name="status">Новый статус платежа</param>
    public sealed record UpdatePaymentStatusCommand(int paymentId, bool status) : IRequest;

    /// <summary>
    /// Обработчик команды обновления статуса платежа
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="notificationService">Сервис уведомлений</param>
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
