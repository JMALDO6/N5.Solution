using N5.Application.Events;

namespace N5.Application.Interfaces
{
    /// <summary>
    /// KafkaProducer interface defines methods for publishing events to Kafka.
    /// </summary>
    public interface IKafkaProducer
    {
        /// <summary>
        /// Publishes a PermissionRequestedEvent to the Kafka topic.
        /// </summary>
        /// <param name="permissionRequestedEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PublishAsync(PermissionNotificationEvent permissionRequestedEvent, CancellationToken cancellationToken);
    }
}
