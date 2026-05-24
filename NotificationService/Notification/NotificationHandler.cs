using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Notification.WebSocketHubs;
using System.Text.Json;

namespace NotificationService.Notification
{
    public sealed record SendNofiticationCommand(Message<string, string> message) : IRequest;

    public class SendNotificationCommandValidator : AbstractValidator<SendNofiticationCommand>
    {
        public SendNotificationCommandValidator()
        {
            RuleFor(x => x.message)
                .NotNull().WithMessage("Сообщение не может быть пустым");

            RuleFor(x => x.message.Key)
                .NotEmpty().WithMessage("Ключ сообщения не может быть пустым");

            RuleFor(x => x.message.Value)
                .NotEmpty().WithMessage("Содержимое сообщения не может быть пустым");
        }
    }

    public class SendNotificationHandler(IHubContext<NotificationHub, INotificationClient> hubContext, NotificationMapper mapper) : IRequestHandler<SendNofiticationCommand>
    {
        public async Task Handle(SendNofiticationCommand request, CancellationToken cancellationToken)
        {
            var key = request.message.Key;
            var rawJson = request.message.Value;

            if (key == "PaymentStatusUpdate")
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var notification = JsonSerializer.Deserialize<KafkaPaymentNotifiaction>(rawJson, options);
                if (notification != null)
                {
                    var paymentNotification = mapper.MapPayment(notification);
                    await hubContext.Clients.All.SendPaymentStatusAsync(paymentNotification);
                }
            }
        }
    }
}
