using Microsoft.Extensions.Logging;
using N5.Application.Events;
using N5.Application.Interfaces;

namespace N5.Application.Helpers
{
    public static class PublishEvents
    {
        /// <summary>
        /// Publishes a permission event to a Kafka topic.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="logger"></param>
        /// <param name="producer"></param>
        /// <param name="operation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task PublishEventInKafkaTopic(Domain.Entities.Permission permission, ILogger logger, IKafkaProducer producer, string operation, CancellationToken cancellationToken)
        {
            logger.LogInformation("Publishing permission event to Kafka topic for employee {EmployeeName} {EmployeeLastName} and operation {Operation}",
               permission.EmployeeForename, permission.EmployeeSurname, operation);
            var evt = new PermissionNotificationEvent
            {
                PermissionId = permission.Id,
                Operation = operation
            };

            await producer.PublishAsync(evt, cancellationToken);
            logger.LogInformation("Permission event published successfully to Kafka topic for employee {EmployeeName} {EmployeeLastName} and operation {Operation}",
                permission.EmployeeForename, permission.EmployeeSurname, operation);
        }
    }
}
