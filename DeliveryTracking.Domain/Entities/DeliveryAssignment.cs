using System;
using DeliveryTracking.Domain.Common;

namespace DeliveryTracking.Domain.Entities
{
    public class DeliveryAssignment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid RiderId { get; set; }
        public AppUser Rider { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
