using MediatR;
using N5.Application.DTOs.Permission;

namespace N5.Application.Commands.Permission.Request
{
    /// <summary>
    /// Represents a command to request a permission.
    /// </summary>
    public class RequestPermissionCommand : IRequest<int>
    {
        /// <summary>
        /// Permission details to be requested.
        /// </summary>
        public required PermissionDto Permission { get; set; }
    }
}
