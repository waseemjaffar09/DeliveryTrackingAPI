using System;
using System.Threading.Tasks;
using DeliveryTracking.Application.Features.Notifications.Commands.RegisterDeviceToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("device-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenRequest request)
        {
            var command = new RegisterDeviceTokenCommand
            {
                Token = request.Token,
                Platform = request.Platform
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new { message = result.Message, data = result.Data });
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }
    }
}
