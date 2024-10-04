using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace ReviewService.Infrastructure.Configurations.EntitiesConfigurations
{
    public class ReviewConfigurations : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnType("UNIQUEIDENTIFIER")
                    .IsRequired();

            builder.Property(x => x.ReviewerId)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(40)
                    .IsRequired();

            builder.Property(x => x.ProductId)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(40)
                    .IsRequired();

            builder.Property(x => x.Rate)
                    .HasColumnType("INTEGER")
                    .IsRequired();

            builder.Property(x => x.CreatedAt)
                    .HasColumnType("DATETIME2")
                    .IsRequired();

            builder.Property(x => x.Comment)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(1024)
                    .IsRequired(false);
        }
    }
}
