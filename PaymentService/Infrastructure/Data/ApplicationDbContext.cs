using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;
using PaymentService.Infrastructure.PaymentServices;

namespace PaymentService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PaypalToken> PaypalTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaypalToken>()
                .ToTable("PaypalTokens");

            modelBuilder.Entity<PaypalToken>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PaypalToken>()
                .Property(x => x.Id)
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            modelBuilder.Entity<PaypalToken>()
                .Property(x => x.Token)
                .HasColumnType("VARCHAR")
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<PaypalToken>()
                .Property(x => x.ExpireDate)
                .HasColumnType("DATETIME2")
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
