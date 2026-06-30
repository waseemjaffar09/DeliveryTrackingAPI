using System;

namespace DeliveryTracking.Application.Features.Orders.DTOs
{
    public class AssignOrderRequest
    {
        public Guid OrderId { get; set; }
        public Guid RiderId { get; set; }
    }
}
