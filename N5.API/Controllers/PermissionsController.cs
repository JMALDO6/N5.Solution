using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Application.Commands.Permission.Modify;
using N5.Application.Commands.Permission.Request;
using N5.Application.DTOs.Permission;
using N5.Application.Queries.Permission.GetAll;
using Serilog;

namespace N5.API.Controllers
{
    /// <summary>
    /// PermissionsController handles permission requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for PermissionsController.
        /// </summary>
        /// <param name="mediator"></param>
        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// RequestPermission allows an employee to request a permission.
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionDto permissionDto)
        {
            try
            {
                Log.Information("Operation: request");

                if (permissionDto is null)
                {
                    Log.Warning("Permission data is required");
                    return BadRequest("Permission data is required.");
                }

                var command = new RequestPermissionCommand { Permission = permissionDto };
                var id = await _mediator.Send(command);
                Log.Information("Permission request processed successfully with ID: {Id}", id);

                return Ok(id);
            }
            catch (Exception)
            {
                Log.Error("An error occurred while processing the permission request.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Update an existing permission.
        /// </summary>
        /// <param name="permissionDto"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ModifyPermission([FromBody] PermissionDto permissionDto)
        {
            try
            {
                Log.Information("Operation: modify");
                if (permissionDto is null || permissionDto.Id <= 0)
                {
                    Log.Warning("Valid permission data with ID is required for modification.");
                    return BadRequest("Valid permission data with ID is required for modification.");
                }

                var command = new ModifyPermissionCommand { Permission = permissionDto };
                var id = await _mediator.Send(command);

                if (id == default)
                {
                    Log.Information("Permission ID not found for modification.");
                    return NotFound("Permission ID not found for modification.");
                }

                Log.Information("Permission modification processed successfully with ID: {Id}", id);

                return Ok(id);
            }
            catch (Exception)
            {
                Log.Error("An error occurred while processing the permission modification.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// GetPermission retrieves a permission by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermission(int id)
        {
            Log.Information("Operation: get by id");
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));

            return result != null ? Ok(result) : NotFound();
        }
    }
}