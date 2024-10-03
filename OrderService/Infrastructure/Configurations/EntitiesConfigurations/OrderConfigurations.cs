using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace OrderService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.Id)
                    .HasColumnType("UNIQUEIDENTIFIER")
                    .IsRequired();

            builder.Property(x => x.UserId)
                    .HasColumnType("UNIQUEIDENTIFIER")
                    .IsRequired();

            builder.Property(x => x.ExternalId)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(40)
                    .IsRequired(false);

            builder.Property(x => x.Status)
                    .HasConversion(
                        x => x.ToString(),
                        x => (Shared.Enums.OrderStatus)Enum.Parse(typeof(Shared.Enums.OrderStatus), x)
                    ).IsRequired();

            builder.Property(x => x.IssuedAt)
                    .HasColumnType("DATETIME2")
                    .IsRequired();

            builder.Property(x => x.Total)
                    .HasColumnType("DECIMAL(10, 2)")
                    .IsRequired();
        }
    }
}
