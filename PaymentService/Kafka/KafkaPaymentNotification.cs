namespace PaymentService.Kafka
{
    /// <summary>
    /// Структура сообщения уведомления о выплате  для Kafka
    /// </summary>
    public record KafkaPaymentNotification
    {
        public required int paymentId { get; init; }
        public required bool status { get; init; }
    }
}
