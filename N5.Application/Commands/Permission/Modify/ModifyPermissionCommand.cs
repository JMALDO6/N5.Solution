using MediatR;
using N5.Application.DTOs.Permission;
using System.Diagnostics.CodeAnalysis;

namespace N5.Application.Commands.Permission.Modify
{
    /// <summary>
    /// ModifyPermissionCommand represents a command to modify an existing permission.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ModifyPermissionCommand : IRequest<int>
    {
        /// <summary>
        /// Permission details to be requested.
        /// </summary>
        public required PermissionDto Permission { get; set; }
    }
}
