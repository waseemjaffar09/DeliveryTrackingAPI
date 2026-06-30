using System;
using System.Threading.Tasks;
using DeliveryTracking.Application.Features.Locations.Commands.UpdateRiderLocation;
using DeliveryTracking.Application.Features.Locations.DTOs;
using DeliveryTracking.Application.Features.Locations.Queries.GetRiderLocation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("update")]
        [Authorize(Policy = "RiderOnly")]
        [ProducesResponseType(typeof(RiderLocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRiderLocation([FromBody] UpdateRiderLocationRequest request)
        {
            var command = new UpdateRiderLocationCommand
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpGet("rider/{riderId}")]
        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(typeof(RiderLocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRiderLocation(Guid riderId)
        {
            var query = new GetRiderLocationQuery(riderId);
            var result = await _mediator.Send(query);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            if (result.Message == "Location not found")
            {
                return NotFound(new { message = result.Message, errors = result.Errors });
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }
    }
}
