using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Persistence.Context;
using DeliveryTracking.Persistence.Repositories;
using DeliveryTracking.Domain.Common;

namespace DeliveryTracking.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found. Please configure it in appsettings.json or environment variables.");
            }

            services.AddDbContext<DeliveryTrackingDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
