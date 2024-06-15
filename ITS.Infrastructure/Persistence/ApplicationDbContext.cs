using ITS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITS.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MvResult>().HasNoKey();
        }

        public DbSet<Inmate> Inmates { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<MvResult> Results { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
    }
}
