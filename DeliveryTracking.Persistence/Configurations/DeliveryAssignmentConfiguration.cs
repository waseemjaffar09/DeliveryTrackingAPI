using System;
using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracking.Persistence.Configurations
{
    public class DeliveryAssignmentConfiguration : IEntityTypeConfiguration<DeliveryAssignment>
    {
        public void Configure(EntityTypeBuilder<DeliveryAssignment> builder)
        {
            builder.ToTable("DeliveryAssignments");

            // Use the inherited Id from BaseEntity
            builder.HasKey(da => da.Id);

            builder.Property(da => da.OrderId).IsRequired();
            builder.Property(da => da.RiderId).IsRequired();
            builder.Property(da => da.AssignedAt).IsRequired();

            builder.HasOne(da => da.Order)
                .WithMany(o => o.DeliveryAssignments)
                .HasForeignKey(da => da.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser does not expose a DeliveryAssignments collection in the domain model,
            // configure the relationship without an inverse navigation property.
            builder.HasOne(da => da.Rider)
                .WithMany()
                .HasForeignKey(da => da.RiderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
