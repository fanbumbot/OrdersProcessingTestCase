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

                    _logger.LogInformation("Notification: {Message}", result.Message.Value);
                    await _hubContext.Clients.All.SendPaymentStatusAsync(result.Message.Value);

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
