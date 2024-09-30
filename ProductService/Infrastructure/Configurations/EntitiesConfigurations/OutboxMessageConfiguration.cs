using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Enums;

namespace ProductService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("Outbox");
            builder.HasKey(x => x.Id);

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

        }
    }
}
