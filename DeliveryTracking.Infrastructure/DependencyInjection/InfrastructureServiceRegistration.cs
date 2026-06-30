using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeliveryTracking.Application.Interfaces.Authentication;
using DeliveryTracking.Application.Interfaces.Identity;
using DeliveryTracking.Application.Interfaces.Notifications;
using DeliveryTracking.Application.Interfaces.Realtime;
using DeliveryTracking.Infrastructure.Authentication;
using DeliveryTracking.Infrastructure.Identity;
using DeliveryTracking.Infrastructure.Notifications;
using DeliveryTracking.Infrastructure.SignalR;

namespace DeliveryTracking.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure JWT Settings
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null)
            {
                throw new InvalidOperationException("JwtSettings configuration is missing. Please configure it in appsettings.json.");
            }

            services.AddSingleton(jwtSettings);

            var firebaseSettings = configuration.GetSection("Firebase").Get<FirebaseSettings>();
            if (firebaseSettings == null)
            {
                throw new InvalidOperationException("Firebase configuration is missing. Please configure it in appsettings.json.");
            }

            services.AddSingleton(firebaseSettings);

            // Register HTTP Context Accessor for Current User Service
            services.AddHttpContextAccessor();

            // Register Authentication Services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRealtimeNotificationService, RealtimeNotificationService>();
            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
