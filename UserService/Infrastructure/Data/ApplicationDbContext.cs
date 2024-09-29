using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using UserService.Infrastructure.Configurations.EntitiesConfigurations;

namespace UserService.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new OutboxMessageConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
