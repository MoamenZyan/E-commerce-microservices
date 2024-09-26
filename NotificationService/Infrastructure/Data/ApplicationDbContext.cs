using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Configurations.EntitiesConfigurations;
using Shared.Entities;

namespace NotificationService.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<NormalNotification> NormalNotifications { get; set; }
        public DbSet<OrderNotification> OrderNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NormalNotificationConfiguration());
            builder.ApplyConfiguration(new OrderNotificationConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
