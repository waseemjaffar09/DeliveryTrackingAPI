using System;
using System.Collections.Generic;
using DeliveryTracking.Domain.Common;
using DeliveryTracking.Domain.Enums;

namespace DeliveryTracking.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Guid? AssignedRiderId { get; set; }
        public AppUser? AssignedRider { get; set; }

        public Guid CreatedByManagerId { get; set; }
        public AppUser CreatedByManager { get; set; } = null!;

        public DateTime? PickupTime { get; set; }
        public DateTime? DeliveryTime { get; set; }

        public ICollection<DeliveryAssignment> DeliveryAssignments { get; set; } = new HashSet<DeliveryAssignment>();
    }
}
