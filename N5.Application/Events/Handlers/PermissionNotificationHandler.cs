using MediatR;
using Microsoft.Extensions.Logging;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.Application.Events.Handlers
{
    /// <summary>
    /// PermissionCreatedHandler listens for PermissionRequestedEvent and indexes the permission in Elasticsearch.
    /// </summary>
    internal class PermissionNotificationHandler : INotificationHandler<PermissionNotificationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionElasticService _elasticClient;
        private readonly ILogger<PermissionNotificationHandler> _logger;

        /// <summary>
        /// Constructor for PermissionCreatedHandler.
        /// </summary>
        /// <param name="elasticClient"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        public PermissionNotificationHandler(IPermissionElasticService elasticClient, IUnitOfWork unitOfWork, ILogger<PermissionNotificationHandler> logger)
        {
            _elasticClient = elasticClient;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Handles the PermissionRequestedEvent to index the permission in Elasticsearch.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(PermissionNotificationEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var permissionDb = await _unitOfWork.Permissions.GetByIdAsync(notification.PermissionId);

                if (permissionDb == null)
                {
                    _logger.LogWarning("Permission with ID {PermissionId} not found in the database.", notification.PermissionId);
                    return;
                }
                
                if (notification.Operation == "request")
                {
                    _logger.LogInformation("Permission with ID {PermissionId} is being indexed in Elasticsearch.", notification.PermissionId);
                    await IndexPermissionInElasticSearch(permissionDb, cancellationToken);
                    _logger.LogInformation("Permission with ID {PermissionId} indexed successfully in Elasticsearch.", notification.PermissionId);
                }
                else if (notification.Operation == "modify")
                {
                    _logger.LogInformation("Permission with ID {PermissionId} is being updated in Elasticsearch.", notification.PermissionId);
                    await UpdatePermissionInElasticSearch(permissionDb, cancellationToken);
                    _logger.LogInformation("Permission with ID {PermissionId} updated successfully in Elasticsearch.", notification.PermissionId);
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while indexing permission with ID {PermissionId} in Elasticsearch.", notification.PermissionId);
                throw;
            }
        }

        /// <summary>
        /// Indexes the permission document in Elasticsearch.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task IndexPermissionInElasticSearch(Permission permission, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexing permission document in Elasticsearch for employee {EmployeeName} {EmployeeLastName}",
               permission.EmployeeForename, permission.EmployeeSurname);

            var document = new PermissionDocument
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate
            };

            await _elasticClient.IndexAsync(document, cancellationToken);
        }

        private async Task UpdatePermissionInElasticSearch(Permission permission, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating permission document in Elasticsearch for employee {EmployeeName} {EmployeeLastName}",
               permission.EmployeeForename, permission.EmployeeSurname);
            var document = new PermissionDocument
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate
            };
            await _elasticClient.UpdateAsync(document, cancellationToken);
        }
    }
}