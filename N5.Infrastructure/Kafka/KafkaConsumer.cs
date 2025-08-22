using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N5.Application.Events;
using System.Text.Json;

namespace N5.Infrastructure.Kafka
{
    /// <summary>
    /// KafkaConsumer is a background service that consumes messages from a Kafka topic.
    /// </summary>
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceScopeFactory _scopeFactory;
        private const string TopicName = "permissions";

        /// <summary>
        /// Ctor for KafkaConsumer.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="consumer"></param>
        /// <param name="scopeFactory"></param>
        public KafkaConsumer(ILogger<KafkaConsumer> logger, IConsumer<Ignore, string> consumer, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _consumer = consumer;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// ExecuteAsync is called by the host to start the background service.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(TopicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(TimeSpan.FromMilliseconds(100));

                    if (result != null)
                    {
                        var permissionEvent = JsonSerializer.Deserialize<PermissionNotificationEvent>(result.Message.Value);
                        using var scope = _scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Publish(permissionEvent, stoppingToken);
                        _consumer.Commit(result);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error while consuming message");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in Kafka consumer");
                }

                await Task.Delay(50, stoppingToken);
            }
        }

        /// <summary>
        /// Dispose is called to clean up resources when the service is stopped.
        /// </summary>
        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}