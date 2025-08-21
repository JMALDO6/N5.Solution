using MediatR;
using Microsoft.Extensions.Logging;
using N5.Domain.Interfaces;

namespace N5.Application.Commands.Permission.Modify
{
    internal class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, int>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<ModifyPermissionHandler> _logger;

        public ModifyPermissionHandler(IPermissionRepository permissionRepository, ILogger<ModifyPermissionHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }
        public async Task<int> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            var messagePrefix = "Executing [ModifyPermissionHandler]";

            try
            {
                _logger.LogInformation("{messagePrefix} Handling ModifyPermissionCommand for employee {EmployeeName} {EmployeeLastName} and ID {ID}",
                     messagePrefix, request.Permission.EmployeeName, request.Permission.EmployeeLastName, request.Permission.Id);

                _logger.LogInformation("{messagePrefix} Search if the permission with ID {ID} exists",
                     messagePrefix, request.Permission.Id);

                var existingPermission = await _permissionRepository.GetByIdAsync(request.Permission.Id);

                if (existingPermission is null)
                {
                    _logger.LogError("{messagePrefix} Permission with ID {ID} not found", messagePrefix, request.Permission.Id);
                    return default;
                }

                existingPermission.PermissionDate = request.Permission.PermissionDate;
                existingPermission.PermissionTypeId = request.Permission.PermissionTypeId;
                existingPermission.EmployeeForename = request.Permission.EmployeeName;
                existingPermission.EmployeeSurname = request.Permission.EmployeeLastName;

                await _permissionRepository.Update(existingPermission);

                _logger.LogInformation("{messagePrefix} Permission updated successfully with ID {PermissionId}", messagePrefix, existingPermission.Id);

                return existingPermission.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{messagePrefix} Error occurred while handling ModifyPermissionCommand for employee {EmployeeName} {EmployeeLastName} and {ID}",
                    messagePrefix, request.Permission.EmployeeName, request.Permission.EmployeeLastName, request.Permission.Id);

                throw new ApplicationException("Error occurred while handling ModifyPermissionCommand", ex);
            }
        }
    }
}
