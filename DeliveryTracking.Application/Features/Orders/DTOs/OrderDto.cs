using System;
using DeliveryTracking.Domain.Enums;

namespace DeliveryTracking.Application.Features.Orders.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public OrderStatus Status { get; set; }
        public Guid? AssignedRiderId { get; set; }
        public Guid CreatedByManagerId { get; set; }
        public DateTime? PickupTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
