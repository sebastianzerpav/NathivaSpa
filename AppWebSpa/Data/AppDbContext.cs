using AppWebSpa.Models;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AppWebSpa.Models.User> User { get; set; } = default!;

        //SpaService
        public DbSet<SpaService> spaService { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpaService>().Property(
                s => s.Price).HasColumnType("decimal(38,2)");
        }

    }
}
