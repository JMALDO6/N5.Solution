using MediatR;
using N5.Application.DTOs.Permission;

namespace N5.Application.Queries.Permission.GetAll
{
    /// <summary>
    /// GetPermissionByIdQuery is a request to retrieve a permission by its ID.
    /// </summary>
    public class GetPermissionByIdQuery : IRequest<PermissionDto>
    {
        /// <summary>
        /// ID of the permission to retrieve.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the permission by its ID.
        /// </summary>
        /// <param name="id"></param>
        public GetPermissionByIdQuery(int id) => Id = id;
    }
}
