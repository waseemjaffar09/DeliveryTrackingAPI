using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<AuthResponse>>
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
