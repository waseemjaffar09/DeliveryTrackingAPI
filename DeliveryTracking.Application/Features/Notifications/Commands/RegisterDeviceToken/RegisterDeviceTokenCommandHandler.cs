using System.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Interfaces.Identity;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    public class RegisterDeviceTokenCommandHandler : IRequestHandler<RegisterDeviceTokenCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<RegisterDeviceTokenCommandHandler> _logger;

        public RegisterDeviceTokenCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<RegisterDeviceTokenCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(RegisterDeviceTokenCommand request, CancellationToken cancellationToken)
        {
            // Verify current user is authenticated
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId == null)
            {
                return Result<bool>.FailureResult(
                    "Unauthorized",
                    new List<string> { "User is not authenticated." }
                );
            }

            // Validate user exists
            var user = await _unitOfWork.Users.GetByIdAsync(_currentUserService.UserId.Value, cancellationToken);
            if (user == null)
            {
                return Result<bool>.FailureResult(
                    "User not found",
                    new List<string> { "The specified user does not exist." }
                );
            }

            // Check if token already exists
            var allDeviceTokens = await _unitOfWork.UserDeviceTokens.GetAllAsync(cancellationToken);
            var existingToken = allDeviceTokens.FirstOrDefault(t => t.Token == request.Token);

            if (existingToken != null)
            {
                // Update existing token
                existingToken.UserId = _currentUserService.UserId.Value;
                existingToken.Platform = request.Platform;
                existingToken.IsActive = true;
                existingToken.LastUpdatedAt = DateTime.UtcNow;

                _unitOfWork.UserDeviceTokens.Update(existingToken);
                _logger.LogInformation("Device token updated for User {UserId}, Platform {Platform}", 
                    _currentUserService.UserId, request.Platform);
            }
            else
            {
                // Create new device token
                var newDeviceToken = new UserDeviceToken
                {
                    UserId = _currentUserService.UserId.Value,
                    Token = request.Token,
                    Platform = request.Platform,
                    IsActive = true,
                    LastUpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.UserDeviceTokens.AddAsync(newDeviceToken, cancellationToken);
                _logger.LogInformation("New device token registered for User {UserId}, Platform {Platform}", 
                    _currentUserService.UserId, request.Platform);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.SuccessResult(true, "Device token registered successfully");
        }
    }
}
