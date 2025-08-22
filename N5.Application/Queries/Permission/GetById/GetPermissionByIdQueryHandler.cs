using MediatR;
using Microsoft.Extensions.Logging;
using N5.Application.DTOs.Permission;
using N5.Application.Helpers;
using N5.Application.Interfaces;
using N5.Domain.Interfaces;

namespace N5.Application.Queries.Permission.GetAll
{
    /// <summary>
    /// GetPermissionByIdQueryHandler handles the retrieval of a permission by its ID.
    /// </summary>
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<GetPermissionByIdQueryHandler> _logger;

        /// <summary>
        /// Ctor for GetPermissionByIdQueryHandler.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="kafkaProducer"></param>
        public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPermissionByIdQueryHandler> logger, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// Handle the GetPermissionByIdQuery to retrieve a permission by its ID.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var messagePrefix = "Executing [GetPermissionByIdQuery]";

            _logger.LogInformation("Handling GetPermissionByIdQuery for ID {ID}", request.Id);

            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);

            if (permission == null)
            {
                _logger.LogWarning("{messagePrefix} Permission with ID {ID} not found", messagePrefix, request.Id);
                return null;
            }

            await PublishEvents.PublishEventInKafkaTopic(permission, _logger, _kafkaProducer, "get", cancellationToken);
            _logger.LogInformation("{messagePrefix} Permission with ID {ID} retrieved successfully", messagePrefix, request.Id);

            return new PermissionDto
            {
                Id = permission.Id,
                EmployeeName = permission.EmployeeForename,
                EmployeeLastName = permission.EmployeeSurname,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate
            };
        }
    }
}