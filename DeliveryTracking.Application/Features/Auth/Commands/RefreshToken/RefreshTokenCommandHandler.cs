using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using DeliveryTracking.Application.Interfaces.Authentication;
using DeliveryTracking.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate and extract principal from expired access token
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.Token);

            if (principal == null)
            {
                _logger.LogWarning("Refresh token request failed: Invalid access token");
                return Result<AuthResponse>.FailureResult(
                    "Invalid token",
                    new List<string> { "The access token is invalid or malformed." }
                );
            }

            // Extract user ID from claims
            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Refresh token request failed: Could not extract user ID from token");
                return Result<AuthResponse>.FailureResult(
                    "Invalid token",
                    new List<string> { "The access token does not contain valid user information." }
                );
            }

            // Get user from database
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Refresh token request failed: User not found for ID {UserId}", userId);
                return Result<AuthResponse>.FailureResult(
                    "User not found",
                    new List<string> { "The user associated with the token does not exist." }
                );
            }

            // Validate refresh token matches
            if (user.RefreshToken != request.RefreshToken)
            {
                _logger.LogWarning("Refresh token request failed for User {UserId}: Refresh token does not match", userId);
                return Result<AuthResponse>.FailureResult(
                    "Invalid refresh token",
                    new List<string> { "The refresh token is invalid or does not match." }
                );
            }

            // Validate refresh token is not expired
            if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token request failed for User {UserId}: Refresh token is expired", userId);
                return Result<AuthResponse>.FailureResult(
                    "Refresh token expired",
                    new List<string> { "The refresh token has expired. Please log in again." }
                );
            }

            // Generate new access token
            var newAccessToken = _jwtTokenService.GenerateToken(user);

            // Generate new refresh token
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 days validity

            // Update user with new refresh token
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token successfully generated for User {UserId}", userId);

            // Return success response with new tokens
            var response = new AuthResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime
            };

            return Result<AuthResponse>.SuccessResult(response, "Tokens refreshed successfully");
        }
    }
}
