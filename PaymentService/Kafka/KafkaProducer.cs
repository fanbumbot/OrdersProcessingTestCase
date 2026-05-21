using Confluent.Kafka;
using System.Text.Json;

namespace PaymentService.Kafka
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(string topic, string key, T message) where T : class;
    }

    /// <summary>
    /// Универсальный продюсер для Kafka
    /// </summary>
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IConfiguration config, ILogger<KafkaProducer> logger)
        {
            _logger = logger;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"],
                Acks = Acks.All,
                EnableIdempotence = true,
                MaxInFlight = 5,
                LingerMs = 5,
                CompressionType = CompressionType.Snappy
            };

            _producer = new ProducerBuilder<string, string>(producerConfig)
                .SetErrorHandler((_, e) => _logger.LogError("Kafka Producer Error: {Reason}, Fatal={IsFatal}", e.Reason, e.IsFatal))
                .Build();
        }

        /// <summary>
        /// Отправить сообщение через Kafka
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="topic">Название топика Kafka</param>
        /// <param name="key">Ключ для Kafka</param>
        /// <param name="message">Сообщение (данные) для передачи</param>
        public async Task PublishAsync<T>(string topic, string key, T message) where T : class
        {
            try
            {
                var jsonValue = JsonSerializer.Serialize(message);
                var kafkaMessage = new Message<string, string> { Key = key, Value = jsonValue };

                var result = await _producer.ProduceAsync(topic, kafkaMessage);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Error message sending in topic {Topic} with key {Key}", topic, key);
                throw;
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
