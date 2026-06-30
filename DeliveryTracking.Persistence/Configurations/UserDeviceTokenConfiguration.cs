using DeliveryTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryTracking.Persistence.Configurations
{
    public class UserDeviceTokenConfiguration : IEntityTypeConfiguration<UserDeviceToken>
    {
        public void Configure(EntityTypeBuilder<UserDeviceToken> builder)
        {
            builder.ToTable("UserDeviceTokens");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Platform)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.LastUpdatedAt)
                .ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(u => u.DeviceTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
