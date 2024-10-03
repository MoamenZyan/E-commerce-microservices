using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderService.Infrastructure.Configurations.EntitiesConfigurations;
using Shared.Entities;

namespace OrderService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfigurations());
            modelBuilder.ApplyConfiguration(new OrderItemConfigurations());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
