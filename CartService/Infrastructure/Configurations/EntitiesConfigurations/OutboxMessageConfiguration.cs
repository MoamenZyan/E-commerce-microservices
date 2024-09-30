using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using Shared.Enums;

namespace CartService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessageCart>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageCart> builder)
        {
            builder.ToTable("Outbox");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.MessageType)
                .HasConversion(
                x => x.ToString(),
                x => (MessageTypes)Enum.Parse(typeof(MessageTypes), x))
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("DATETIME2")
                .HasDefaultValue(DateTime.Now)
                .IsRequired();

            builder.Property(x => x.Processed)
                .HasColumnType("BIT")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.IsNotification)
                .HasColumnType("BIT")
                .IsRequired();

        }
    }
}
