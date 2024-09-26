using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class NormalNotificationConfiguration : IEntityTypeConfiguration<NormalNotification>
    {
        public void Configure(EntityTypeBuilder<NormalNotification> builder)
        {
            builder.ToTable("NormalNotifications");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                     .HasColumnType("UNIQUEIDENTIFIER")
                     .HasMaxLength(40)
                     .IsRequired();

            builder.Property(x => x.CreatedAt)
                    .HasColumnType("DATETIME2")
                    .IsRequired();

            builder.Property(x => x.Body)
                    .HasColumnType("NVARCHAR")
                    .HasMaxLength(4000)
                    .IsRequired();
        }
    }
}
