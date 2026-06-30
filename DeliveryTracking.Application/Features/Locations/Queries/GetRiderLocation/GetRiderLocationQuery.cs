using System;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Locations.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Locations.Queries.GetRiderLocation
{
    public class GetRiderLocationQuery : IRequest<Result<RiderLocationDto>>
    {
        public Guid RiderId { get; set; }

        public GetRiderLocationQuery(Guid riderId)
        {
            RiderId = riderId;
        }
    }
}
