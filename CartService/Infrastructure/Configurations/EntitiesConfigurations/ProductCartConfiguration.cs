using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class ProductCartConfiguration : IEntityTypeConfiguration<ProductCart>
    {
        public void Configure(EntityTypeBuilder<ProductCart> builder)
        {
            builder.ToTable("ProductCarts");
            builder.HasKey(x => new { x.ProductId, x.CartId });

            builder.Property(x => x.Quantity)
                .HasColumnType("INTEGER")
                .IsRequired();
        }
    }
}
