using DeliveryTracking.Domain.Common;

namespace DeliveryTracking.Domain.Entities
{
    public class UserDeviceToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
