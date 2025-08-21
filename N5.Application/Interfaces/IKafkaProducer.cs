using N5.Application.Events;

namespace N5.Application.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishAsync(PermissionRequestedEvent evt);
    }
}
