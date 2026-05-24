namespace PaymentService.Kafka
{
    /// <summary>
    /// Структура сообщения уведомления о выплате  для Kafka
    /// </summary>
    public record KafkaNotification
    {
        public required int paymentId { get; init; }
        public required bool status { get; init; }
    }
}
