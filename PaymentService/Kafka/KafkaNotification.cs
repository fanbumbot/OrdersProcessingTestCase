namespace PaymentService.Kafka
{
    public record KafkaNotification
    {
        public required int paymentId { get; init; }
        public required bool status { get; init; }
    }
}
