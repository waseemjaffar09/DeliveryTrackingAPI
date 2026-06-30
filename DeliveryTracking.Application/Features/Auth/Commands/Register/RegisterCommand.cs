using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Result<AuthResponse>>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int Role { get; set; }
    }
}
