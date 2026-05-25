using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Notification.WebSocketHubs;
using System.Text.Json;

namespace NotificationService.Notification
{
    /// <summary>
    /// Команда для отправки уведомлений клиентам
    /// </summary>
    /// <param name="message">Сообщение</param>
    public sealed record SendNofiticationCommand(Message<string, string> message) : IRequest;

    /// <summary>
    /// Валидатор для уведомлений
    /// </summary>
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

    /// <summary>
    /// Обработчик команды для отправки уведомлений клиентам
    /// </summary>
    /// <param name="hubContext">Hub из SignalR для WebSocket</param>
    /// <param name="mapper">Маппер уведомлений</param>
    public class SendNotificationHandler(IHubContext<NotificationHub, INotificationClient> hubContext, NotificationMapper mapper) : IRequestHandler<SendNofiticationCommand>
    {
        /// <summary>
        /// Обработка команды для отправки уведомлений клиентам
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        public async Task Handle(SendNofiticationCommand request, CancellationToken cancellationToken)
        {
            var key = request.message.Key;
            var rawJson = request.message.Value;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            if (key == "PaymentStatusUpdate")
            {
                var notification = JsonSerializer.Deserialize<KafkaPaymentNotifiaction>(rawJson, options);
                if (notification != null)
                {
                    var paymentNotification = mapper.MapPayment(notification);
                    await hubContext.Clients.All.SendPaymentStatusAsync(paymentNotification);
                }
            }
            if (key == "OrderCreate")
            {
                var notification = JsonSerializer.Deserialize<KafkaOrderNotifiaction>(rawJson, options);
                if (notification != null)
                {
                    var orderNotification = mapper.MapOrder(notification);
                    await hubContext.Clients.All.SendOrderAsync(orderNotification);
                }
            }
        }
    }
}
