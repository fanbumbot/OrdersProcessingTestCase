namespace NotificationService.Kafka
{
    using Confluent.Kafka;
    using Confluent.Kafka.Admin;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class KafkaInitializer : IHostedService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<KafkaInitializer> _logger;
        private readonly string _topic = "notifications";

        public KafkaInitializer(IConfiguration config, ILogger<KafkaInitializer> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Kafka initialization");

            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"]
            };

            using var adminClient = new AdminClientBuilder(adminConfig)
                .SetErrorHandler((_, error) => _logger.LogError("AdminClient Error: {Reason}", error.Reason))
                .Build();

            try
            {
                var topicSpecification = new TopicSpecification
                {
                    Name = _topic,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                };

                await adminClient.CreateTopicsAsync(new[] { topicSpecification });
                _logger.LogInformation("Topic '{Topic}' has been created", _topic);
            }
            catch (CreateTopicsException e) when (e.Results.Any(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
            {
                _logger.LogInformation("Topic '{Topic}' is already existed. Skip", _topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error with Kafka initialization: {Message}", ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
