using System;
using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracking.Persistence.Configurations
{
    public class RiderLocationConfiguration : IEntityTypeConfiguration<RiderLocation>
    {
        public void Configure(EntityTypeBuilder<RiderLocation> builder)
        {
            builder.ToTable("RiderLocations");

            // Use the inherited Id from BaseEntity
            builder.HasKey(rl => rl.Id);

            builder.Property(rl => rl.RiderId).IsRequired();

            builder.Property(rl => rl.Latitude)
                .HasColumnType("decimal(18,8)");

            builder.Property(rl => rl.Longitude)
                .HasColumnType("decimal(18,8)");

            builder.Property(rl => rl.UpdatedAt).IsRequired();

            builder.HasOne(rl => rl.Rider)
                .WithMany(u => u.Locations)
                .HasForeignKey(rl => rl.RiderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
