using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Application.Commands;
using N5.Application.DTOs;

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
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// RequestPermission allows an employee to request a permission.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionDto dto)
        {
            _logger.LogInformation("Operation: request");
            var command = new RequestPermissionCommand { Permission = dto };
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(RequestPermission), new { id }, dto);
        }
    }

}
