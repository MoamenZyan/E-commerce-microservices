using Microsoft.EntityFrameworkCore;
using ReviewService.Controllers;
using Shared.Entities;

namespace ReviewService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewController).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
