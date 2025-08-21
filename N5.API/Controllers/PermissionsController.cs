using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Application.Commands.Permission.Request;
using N5.Application.DTOs.Permission;
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
    }
}
