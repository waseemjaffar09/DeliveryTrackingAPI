using System;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using DeliveryTracking.Application.Interfaces.Authentication;
using DeliveryTracking.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            ILogger<LoginCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Get all users and find by email
            var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            var user = users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found with email {Email}", request.Email);
                return Result<AuthResponse>.FailureResult(
                    "Invalid credentials",
                    new List<string> { "Email or password is incorrect." }
                );
            }

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for email {Email}: Invalid password", request.Email);
                return Result<AuthResponse>.FailureResult(
                    "Invalid credentials",
                    new List<string> { "Email or password is incorrect." }
                );
            }

            // Check if user is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed for email {Email}: User account is inactive", request.Email);
                return Result<AuthResponse>.FailureResult(
                    "User account is inactive",
                    new List<string> { "Your account has been disabled." }
                );
            }

            // Generate access token
            var accessToken = _jwtTokenService.GenerateToken(user);

            // Generate refresh token
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 days validity

            // Save refresh token to user
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} logged in successfully with email {Email}", user.Id, user.Email);

            // Return success response
            var response = new AuthResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime
            };

            return Result<AuthResponse>.SuccessResult(response, "Login successful");
        }
    }
}
