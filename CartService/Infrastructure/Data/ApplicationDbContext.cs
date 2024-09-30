using CartService.Domain.Entities;
using CartService.Infrastructure.Configurations.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace CartService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<OutboxMessageCart> OutboxMessages { get; set; }
        public DbSet<ProductCart> ProductCarts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCartConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
