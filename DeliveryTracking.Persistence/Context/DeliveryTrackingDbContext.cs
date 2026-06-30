using System;
using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryTracking.Persistence.Context
{
    public class DeliveryTrackingDbContext : DbContext
    {
        public DeliveryTrackingDbContext(DbContextOptions<DeliveryTrackingDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; } = null!;
        public DbSet<RiderLocation> RiderLocations { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<UserDeviceToken> UserDeviceTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryTrackingDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
