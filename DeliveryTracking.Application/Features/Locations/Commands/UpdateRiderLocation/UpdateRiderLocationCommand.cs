using System;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Locations.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Locations.Commands.UpdateRiderLocation
{
    public class UpdateRiderLocationCommand : IRequest<Result<RiderLocationDto>>
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
