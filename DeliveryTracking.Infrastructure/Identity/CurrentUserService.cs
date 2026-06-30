using System;
using System.Security.Claims;
using DeliveryTracking.Application.Interfaces.Identity;
using DeliveryTracking.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DeliveryTracking.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return null;
                }
                return userId;
            }
        }

        public string? Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        public string? FullName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            }
        }

        public UserRole? Role
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(roleClaim))
                {
                    return null;
                }

                if (Enum.TryParse<UserRole>(roleClaim, out var role))
                {
                    return role;
                }

                // Try parsing as integer (in case the role is stored as numeric string)
                if (int.TryParse(roleClaim, out var roleInt) && Enum.IsDefined(typeof(UserRole), roleInt))
                {
                    return (UserRole)roleInt;
                }

                return null;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
            }
        }
    }
}
