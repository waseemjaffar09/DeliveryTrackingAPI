using System;
using System.Collections.Generic;
using DeliveryTracking.Domain.Common;
using DeliveryTracking.Domain.Enums;

namespace DeliveryTracking.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<Order> CreatedOrders { get; set; } = new HashSet<Order>();
        public ICollection<Order> AssignedOrders { get; set; } = new HashSet<Order>();
        public ICollection<RiderLocation> Locations { get; set; } = new HashSet<RiderLocation>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<UserDeviceToken> DeviceTokens { get; set; } = new HashSet<UserDeviceToken>();
    }
}
