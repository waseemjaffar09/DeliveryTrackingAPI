using System;
using DeliveryTracking.Domain.Common;

namespace DeliveryTracking.Domain.Entities
{
    public class RiderLocation : BaseEntity
    {
        public Guid RiderId { get; set; }
        public AppUser Rider { get; set; } = null!;

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
