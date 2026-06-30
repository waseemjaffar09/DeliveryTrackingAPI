using DeliveryTracking.Domain.Entities;
using System.Security.Claims;

namespace DeliveryTracking.Application.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
