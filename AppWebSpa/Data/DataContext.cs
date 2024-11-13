using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<CategoryService> CategoryServices { get; set; }
        public DbSet<User> User { get; set; } = default!;

        public DbSet<SpaService> spaService { get; set; }
        public DbSet<RolesForUser> rolesForUser{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PK
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new { x.LoginProvider, x.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            // SpaServices
            modelBuilder.Entity<SpaService>().Property(s => s.Price)
                .HasColumnType("decimal(38,2)");
        }


    }
}
