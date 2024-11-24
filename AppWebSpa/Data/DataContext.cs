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
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<SpaService> spaService { get; set; }
        public DbSet<NathivaRole> NathivaRoles{ get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RoleCategory> RoleCategories { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfiguredKeys(builder);
            ConfigureIndexes(builder);

            base.OnModelCreating(builder);

            // PK
            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new { x.LoginProvider, x.ProviderKey });

            builder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId, x.RoleId });

            builder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            // SpaServices
            builder.Entity<SpaService>().Property(s => s.Price)
                .HasColumnType("decimal(38,0)");
        }

        //Index: son restricciones a las tablas en la base de datos, en este caso campos de las tablas que deben ser unicos
        private void ConfigureIndexes(ModelBuilder builder)
        {
            //Roles
            builder.Entity<NathivaRole>().HasIndex(r => r.Name).IsUnique();

            //Categories
            builder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

            //Users
            builder.Entity<User>().HasIndex(u => u.Document).IsUnique();
        }

        private void ConfiguredKeys(ModelBuilder builder)
        {
            //Role Permissions Clave foranea compuesta
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                            .WithMany(r => r.RolePermisions)
                                            .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermisions)
                                            .HasForeignKey(rp => rp.PermissionId);

            //RoleCategories
            builder.Entity<RoleCategory>().HasKey(rs => new { rs.RoleId, rs.CategoryId });
            builder.Entity<RoleCategory>().HasOne(rs => rs.Role)
                                            .WithMany(r => r.RoleCategories)
                                            .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RoleCategory>().HasOne(rs => rs.Category)
                                            .WithMany(p => p.RoleCategories)
                                            .HasForeignKey(rs => rs.CategoryId);
        }

        

    }
}
