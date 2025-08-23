using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5.Application.Interfaces;
using N5.Domain.Interfaces;
using N5.Infrastructure.Elasticsearch;
using N5.Infrastructure.Kafka;
using Nest;

namespace IntegrationTests.Fixtures
{
    public class ApiTestFixture : WebApplicationFactory<Program>
    {
        public IElasticClient ElasticClient { get; private set; }
        public IKafkaProducer KafkaProducer { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                { "Kafka:BootstrapServers", "localhost:9092" },
                { "Elasticsearch:Url", "http://localhost:9200" },
                { "Elasticsearch:Index", "permissions" }
                });
            });

            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                // Elasticsearch
                var elasticSettings = new ConnectionSettings(new Uri(configuration["Elasticsearch:Url"]))
                    .DefaultIndex(configuration["Elasticsearch:Index"]);

                ElasticClient = new ElasticClient(elasticSettings);
                services.AddSingleton<IElasticClient>(ElasticClient);

                // Kafka Producer
                var kafkaConfig = new ProducerConfig
                {
                    BootstrapServers = configuration["Kafka:BootstrapServers"]
                };
                var producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
                services.AddSingleton<IProducer<string, string>>(producer);
                services.AddScoped<IKafkaProducer, KafkaProducer>();

                // Kafka Consumer
                using var scope = services.BuildServiceProvider().CreateScope();
                KafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = configuration["Kafka:BootstrapServers"],
                    GroupId = "test-consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
                services.AddSingleton<IConsumer<string, string>>(consumer);
                services.AddHostedService<KafkaConsumer>();
                services.AddScoped<IPermissionElasticService, PermissionElasticService>();
            });
        }
    }
}