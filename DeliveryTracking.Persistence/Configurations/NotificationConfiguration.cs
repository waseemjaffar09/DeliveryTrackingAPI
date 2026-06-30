using System;
using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracking.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            // Use the inherited Id from BaseEntity
            builder.HasKey(n => n.Id);

            builder.Property(n => n.UserId).IsRequired();

            builder.Property(n => n.Title)
                .HasMaxLength(150);

            builder.Property(n => n.Message)
                .HasMaxLength(500);

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false);

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
