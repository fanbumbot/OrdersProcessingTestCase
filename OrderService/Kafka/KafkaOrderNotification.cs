namespace OrderService.Kafka
{
    /// <summary>
    /// Структура сообщения уведомления о создании заказа  для Kafka
    /// </summary>
    public record KafkaOrderNotification
    {
        public required int orderId { get; init; }
    }
}
