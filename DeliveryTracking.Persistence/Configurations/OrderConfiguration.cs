using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracking.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.Property(o => o.CustomerName)
                .HasMaxLength(150);

            builder.Property(o => o.CustomerPhone)
                .HasMaxLength(50);

            builder.Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.Status)
                .HasConversion<int>();

            builder.Property(x => x.Latitude)
                .HasColumnType("decimal(18,8)");

            builder.Property(x => x.Longitude)
                .HasColumnType("decimal(18,8)");

            builder.HasOne(o => o.CreatedByManager)
                .WithMany(u => u.CreatedOrders)
                .HasForeignKey(o => o.CreatedByManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.AssignedRider)
                .WithMany(u => u.AssignedOrders)
                .HasForeignKey(o => o.AssignedRiderId)
                .OnDelete(DeleteBehavior.SetNull);

            // PickupTime and DeliveryTime are optional by design (nullable DateTime?)
        }
    }
}
