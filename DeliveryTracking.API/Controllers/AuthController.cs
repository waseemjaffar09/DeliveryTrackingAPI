using System;
using System.Threading.Tasks;
using DeliveryTracking.Application.Features.Auth.Commands.Login;
using DeliveryTracking.Application.Features.Auth.Commands.Register;
using DeliveryTracking.Application.Features.Auth.Commands.RefreshToken;
using DeliveryTracking.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return Unauthorized(new { message = result.Message, errors = result.Errors });
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand
            {
                Token = request.Token,
                RefreshToken = request.RefreshToken
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return Unauthorized(new { message = result.Message, errors = result.Errors });
        }
    }
}
