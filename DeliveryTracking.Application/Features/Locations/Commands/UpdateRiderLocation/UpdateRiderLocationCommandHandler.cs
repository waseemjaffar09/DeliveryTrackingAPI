using System;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Locations.DTOs;
using DeliveryTracking.Application.Interfaces.Identity;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Application.Interfaces.Realtime;
using DeliveryTracking.Domain.Entities;
using DeliveryTracking.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Locations.Commands.UpdateRiderLocation
{
    public class UpdateRiderLocationCommandHandler : IRequestHandler<UpdateRiderLocationCommand, Result<RiderLocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRealtimeNotificationService _realtimeNotificationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateRiderLocationCommandHandler> _logger;

        public UpdateRiderLocationCommandHandler(
            IUnitOfWork unitOfWork,
            IRealtimeNotificationService realtimeNotificationService,
            ICurrentUserService currentUserService,
            ILogger<UpdateRiderLocationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _realtimeNotificationService = realtimeNotificationService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<Result<RiderLocationDto>> Handle(UpdateRiderLocationCommand request, CancellationToken cancellationToken)
        {
            // Verify current user is authenticated
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId == null)
            {
                return Result<RiderLocationDto>.FailureResult(
                    "Unauthorized",
                    new List<string> { "User is not authenticated." }
                );
            }

            // Validate rider exists
            var rider = await _unitOfWork.Users.GetByIdAsync(_currentUserService.UserId.Value, cancellationToken);
            if (rider == null)
            {
                return Result<RiderLocationDto>.FailureResult(
                    "Rider not found",
                    new List<string> { "The specified rider does not exist." }
                );
            }

            // Validate user role is Rider
            if (rider.Role != UserRole.Rider)
            {
                return Result<RiderLocationDto>.FailureResult(
                    "Invalid user role",
                    new List<string> { "Only riders can update their location." }
                );
            }

            // Get all rider locations to find existing record
            var allLocations = await _unitOfWork.RiderLocations.GetAllAsync(cancellationToken);
            var existingLocation = allLocations.FirstOrDefault(rl => rl.RiderId == _currentUserService.UserId.Value);

            RiderLocation riderLocation;

            if (existingLocation != null)
            {
                // Update existing location
                existingLocation.Latitude = request.Latitude;
                existingLocation.Longitude = request.Longitude;
                existingLocation.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.RiderLocations.Update(existingLocation);
                riderLocation = existingLocation;
            }
            else
            {
                // Create new location record
                riderLocation = new RiderLocation
                {
                    RiderId = _currentUserService.UserId.Value,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    UpdatedAt = DateTime.UtcNow
                };
                await _unitOfWork.RiderLocations.AddAsync(riderLocation, cancellationToken);
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Rider {RiderId} location updated: Latitude {Latitude}, Longitude {Longitude}", 
                riderLocation.RiderId, riderLocation.Latitude, riderLocation.Longitude);

            await _realtimeNotificationService.SendRiderLocationToManagers(
                riderLocation.RiderId,
                riderLocation.Latitude,
                riderLocation.Longitude,
                riderLocation.UpdatedAt);

            // Map to DTO and return
            var dto = MapToRiderLocationDto(riderLocation);
            return Result<RiderLocationDto>.SuccessResult(dto, "Rider location updated successfully");
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
