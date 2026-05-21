using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.WebAPI.Hubs;
using System.Text.Json;
using System.Threading;

namespace NotificationService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _topic;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public KafkaConsumer(
            IConfiguration config,
            ILogger<KafkaConsumer> logger,
            IServiceProvider serviceProvider,
            IHubContext<NotificationHub, INotificationClient> hubContext
        )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _topic = "notifications";
            _hubContext = hubContext;

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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _consumer.Subscribe(_topic);
                    _logger.LogInformation("Kafka consumer started with topic {Topic}", _topic);
                    break;
                }
                catch (ConsumeException ex)
                {
                    _logger.LogWarning("Kafka topic is not ready, resubscribe in 3 seconds");
                    Thread.Sleep(3000);
                }
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<string, string>? result = null;
                try
                {
                    //result = _consumer.Consume(TimeSpan.FromMilliseconds(500));
                    result = await Task.Run(() => _consumer.Consume(TimeSpan.FromMilliseconds(500)), stoppingToken);

                    if (result == null)
                    {
                        continue;
                    }

                    _logger.LogInformation("New Message!: {Message}", result.Message.Value);
                    _hubContext.Clients.All.ReceivePaymentStatusAsync(result.Message.Value);

                    _consumer.Commit(result);
                }
                catch (OperationCanceledException)
                {
                    break;
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
