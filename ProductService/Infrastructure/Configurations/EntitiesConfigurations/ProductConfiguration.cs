using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DTOs;
using Shared.Entities;
using Shared.Enums;

namespace ProductService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductDto>
    {
        public void Configure(EntityTypeBuilder<ProductDto> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired();

            builder.Property(x => x.Discount)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(x => x.Category)
                .HasConversion(
                        x => x.ToString(),
                        x => (CategoryTypes)Enum.Parse(typeof(CategoryTypes), x)
                )
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("DECIMAL(10, 2)")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("DATETIME2")
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();
        }
    }
}
