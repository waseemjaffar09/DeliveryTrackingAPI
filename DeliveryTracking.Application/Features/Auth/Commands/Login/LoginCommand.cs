using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
