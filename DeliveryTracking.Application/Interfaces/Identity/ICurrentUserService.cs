using DeliveryTracking.Domain.Enums;

namespace DeliveryTracking.Application.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? FullName { get; }
        UserRole? Role { get; }
        bool IsAuthenticated { get; }
    }
}
