using MediatR;
using Microsoft.Extensions.Logging;
using N5.Application.Events;
using N5.Application.Helpers;
using N5.Application.Interfaces;
using N5.Domain.Interfaces;

namespace N5.Application.Commands.Permission.Request
{
    /// <summary>
    /// RequestPermissionHandler handles the RequestPermissionCommand to process permission requests.
    /// </summary>
    internal class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<RequestPermissionHandler> _logger;

        /// <summary>
        /// Constructor for RequestPermissionHandler.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="kafkaProducer"></param>    
        public RequestPermissionHandler(IUnitOfWork unitOfWork, ILogger<RequestPermissionHandler> logger, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// Handle the RequestPermissionCommand to create a new permission request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            var messagePrefix = "Executing [RequestPermissionHandler]";

            try
            {
                _logger.LogInformation("Handling RequestPermissionCommand for employee {EmployeeName} {EmployeeLastName}",
                     request.Permission.EmployeeName, request.Permission.EmployeeLastName);

                var permission = new Domain.Entities.Permission
                {
                    EmployeeForename = request.Permission.EmployeeName,
                    EmployeeSurname = request.Permission.EmployeeLastName,
                    PermissionTypeId = request.Permission.PermissionTypeId,
                    PermissionDate = request.Permission.PermissionDate
                };
                await _unitOfWork.Permissions.AddAsync(permission);
                await _unitOfWork.SaveChangesAsync();
                await PublishEvents.PublishEventInKafkaTopic(permission, _logger, _kafkaProducer, "request", cancellationToken);

                _logger.LogInformation("{messagePrefix} Permission created successfully with ID {PermissionId}", messagePrefix, permission.Id);

                return permission.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{messagePrefix} Error occurred while handling RequestPermissionCommand for employee {EmployeeName} {EmployeeLastName}",
                    messagePrefix, request.Permission.EmployeeName, request.Permission.EmployeeLastName);

                throw new ApplicationException("Error occurred while handling RequestPermissionCommand", ex);
            }
        }
    }
}