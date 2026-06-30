using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Locations.DTOs;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using MediatR;

namespace DeliveryTracking.Application.Features.Locations.Queries.GetRiderLocation
{
    public class GetRiderLocationQueryHandler : IRequestHandler<GetRiderLocationQuery, Result<RiderLocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRiderLocationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RiderLocationDto>> Handle(GetRiderLocationQuery request, CancellationToken cancellationToken)
        {
            // Validate rider exists
            var rider = await _unitOfWork.Users.GetByIdAsync(request.RiderId, cancellationToken);
            if (rider == null)
            {
                return Result<RiderLocationDto>.FailureResult(
                    "Rider not found",
                    new List<string> { "The specified rider does not exist." }
                );
            }

            // Get all rider locations and find the one for this rider
            var allLocations = await _unitOfWork.RiderLocations.GetAllAsync(cancellationToken);
            var riderLocation = allLocations.FirstOrDefault(rl => rl.RiderId == request.RiderId);

            if (riderLocation == null)
            {
                return Result<RiderLocationDto>.FailureResult(
                    "Location not found",
                    new List<string> { "No location data available for this rider." }
                );
            }

            // Map to DTO and return
            var dto = MapToRiderLocationDto(riderLocation);
            return Result<RiderLocationDto>.SuccessResult(dto);
        }

        private RiderLocationDto MapToRiderLocationDto(RiderLocation location)
        {
            return new RiderLocationDto
            {
                RiderId = location.RiderId,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                UpdatedAt = location.UpdatedAt
            };
        }
    }
}
