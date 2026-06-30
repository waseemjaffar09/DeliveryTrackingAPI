using System;

namespace DeliveryTracking.Application.Features.Locations.DTOs
{
    public class RiderLocationDto
    {
        public Guid RiderId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
