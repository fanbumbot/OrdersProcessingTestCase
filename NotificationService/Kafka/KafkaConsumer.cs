using Confluent.Kafka;
using NotificationService.Notification;

namespace NotificationService.Kafka
{
    /// <summary>
    /// Универсальный Kafka consumer
    /// Асинхронно получает сообщения из Kafka
    /// </summary>
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _topic;

        private readonly INotificationHandler _handler;

        public KafkaConsumer(
            IConfiguration config,
            ILogger<KafkaConsumer> logger,
            IServiceProvider serviceProvider,
            INotificationHandler handler
        )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _topic = "notifications";
            _handler = handler;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"],
                GroupId = "notifications-streaming-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,

                SessionTimeoutMs = 10000
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetErrorHandler((_, e) => _logger.LogError("Kafka Error: {Reason}, Fatal={IsFatal}", e.Reason, e.IsFatal))
                .Build();
        }

        /// <summary>
        /// Запустить асинхрноо цикл получения информации из Kafka
        /// </summary>
        /// <param name="stoppingToken">Переменная для остановки задачи</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);

            _consumer.Subscribe(_topic);
            _logger.LogInformation("Kafka consumer started with topic {Topic}", _topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = await Task.Run(() => _consumer.Consume(stoppingToken), stoppingToken);

                    if (result == null)
                    {
                        continue;
                    }

                    await _handler.HandleRawNotificationAsync(result.Message);

                    _consumer.Commit(result);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException ex)
                {
                    if (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                    {
                        _logger.LogWarning("Topic {Topic} not found", _topic);
                    }
                    else
                    {
                        _logger.LogError("Kafka Consume Error: {Reason}", ex.Error.Reason);
                    }
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error with Kafka message handling. The queue has been paused for 5 seconds");
                    await Task.Delay(3000, stoppingToken);
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
