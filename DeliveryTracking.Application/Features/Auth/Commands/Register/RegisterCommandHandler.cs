using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Auth.DTOs;
using DeliveryTracking.Application.Interfaces.Authentication;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using DeliveryTracking.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            ILogger<RegisterCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            if (existingUsers.Any(u => u.Email == request.Email))
            {
                _logger.LogWarning("Registration failed for email {Email}: Email already registered", request.Email);
                return Result<AuthResponse>.FailureResult(
                    "Email already registered",
                    new List<string> { "This email is already in use." }
                );
            }

            // Create new user
            var newUser = new AppUser
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Role = (UserRole)request.Role,
                IsActive = true
            };

            // Add user to repository
            await _unitOfWork.Users.AddAsync(newUser, cancellationToken);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Generate token
            var token = _jwtTokenService.GenerateToken(newUser);

            _logger.LogInformation("User {UserId} registered successfully with email {Email} and role {Role}", newUser.Id, newUser.Email, newUser.Role);

            // Return success response
            var response = new AuthResponse
            {
                UserId = newUser.Id,
                FullName = newUser.FullName,
                Email = newUser.Email,
                Role = newUser.Role,
                Token = token
            };

            return Result<AuthResponse>.SuccessResult(response, "User registered successfully");
        }
    }
}
