using MediatR;
using Microsoft.Extensions.Logging;
using N5.Domain.Interfaces;

namespace N5.Application.Commands.Permission.Request
{
    /// <summary>
    /// RequestPermissionHandler handles the RequestPermissionCommand to process permission requests.
    /// </summary>
    internal class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<RequestPermissionHandler> _logger;

        /// <summary>
        /// Constructor for RequestPermissionHandler.
        /// </summary>
        /// <param name="permissionRepository"></param>
        public RequestPermissionHandler(IPermissionRepository permissionRepository, ILogger<RequestPermissionHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handle the RequestPermissionCommand to create a new permission request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
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
                await _permissionRepository.AddAsync(permission);

                _logger.LogInformation("Permission created successfully with ID {PermissionId}", permission.Id);

                return permission.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling RequestPermissionCommand for employee {EmployeeName} {EmployeeLastName}",
                    request.Permission.EmployeeName, request.Permission.EmployeeLastName);

                throw new ApplicationException("Error occurred while handling RequestPermissionCommand", ex);
            }
        }
    }
}
