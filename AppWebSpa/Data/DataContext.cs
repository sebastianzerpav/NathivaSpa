using AppWebSpa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<CategoryService> CategoryServices { get; set; }
        public DbSet<User> User { get; set; } = default!;

        //SpaService
        public DbSet<SpaService> spaService { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpaService>().Property(
                s => s.Price).HasColumnType("decimal(38,2)");
        }

    }
}
