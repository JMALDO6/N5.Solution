using Confluent.Kafka;
using N5.Application.Events;
using N5.Application.Interfaces;
using Newtonsoft.Json;

namespace N5.Infrastructure.Kafka
{
    /// <summary>
    /// KafkaProducer is responsible for publishing events to a Kafka topic.
    /// </summary>
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        private const string TopicName = "permissions";

        /// <summary>
        /// Constructor for KafkaProducer.
        /// </summary>
        /// <param name="producer"></param>
        public KafkaProducer(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        /// <summary>
        /// Publishes a PermissionRequestedEvent to the Kafka topic.
        /// </summary>
        /// <param name="permissionRequestedEvent"></param>
        /// <returns></returns>
        public async Task PublishAsync(PermissionNotificationEvent permissionRequestedEvent, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(permissionRequestedEvent);

            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            };

            await _producer.ProduceAsync(TopicName, message, cancellationToken);
        }
    }
}
