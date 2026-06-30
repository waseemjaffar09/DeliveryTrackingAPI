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
        // Migrations assemblies per provider. Each provider keeps its own migrations
        // and model snapshot in a dedicated assembly so both can coexist.
        private const string PostgreSqlMigrationsAssembly = "DeliveryTracking.Persistence";
        private const string SqlServerMigrationsAssembly = "DeliveryTracking.Persistence.SqlServer";

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found. Please configure it in appsettings.json or environment variables.");
            }

            // Choose the database provider. Defaults to PostgreSQL (used on Render).
            // Override via the "DatabaseProvider" environment variable or appsettings
            // value: "PostgreSQL" or "SqlServer".
            var provider = Environment.GetEnvironmentVariable("DatabaseProvider")
                ?? configuration["DatabaseProvider"]
                ?? "PostgreSQL";

            services.AddDbContext<DeliveryTrackingDbContext>(options =>
            {
                if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlServer(connectionString, sql =>
                        sql.MigrationsAssembly(SqlServerMigrationsAssembly));
                }
                else
                {
                    options.UseNpgsql(connectionString, npgsql =>
                        npgsql.MigrationsAssembly(PostgreSqlMigrationsAssembly));
                }
            });

            // Register generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
