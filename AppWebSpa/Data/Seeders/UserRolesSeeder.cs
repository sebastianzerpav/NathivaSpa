using AppWebSpa.Core;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public UserRolesSeeder(DataContext context, IUsersService userService)
        {
            _context = context;
            _usersService = userService;
        }

        public async Task SeedAsync() {
            await CheckRoles();
            await CheckUsers();
        }
        private async Task CheckUsers()
        {
            //Administrador
            User? user = await _usersService.GetUserAsync("cathe@yopmail.com");
            
            if (user == null)
            {
                NathivaRole adminRole = _context.NathivaRoles.FirstOrDefault(r => r.Name==Env.SUPER_ADMIN_ROLE_NAME);
               
                user = new User
                {
                    Email = "cathe@yopmail.com",
                    Name = "Cathe",
                    PhoneNumber = "30000000",
                    UserName = "cathe@yopmail.com",
                    Document="11111",
                    NathivaRole= adminRole
                };

                //var result = await _usersService.AddUserAsync(user, "admin");
                await _usersService.AddUserAsync(user, "12345");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            //Content Manager
            user = await _usersService.GetUserAsync("saul@yopmail.com");

            if (user == null)
            {
                NathivaRole contentManagerRole = _context.NathivaRoles.FirstOrDefault(r => r.Name == "Gestor de contenido");

                user = new User
                {
                    Email = "saul@yopmail.com",
                    Name = "Saul",
                    PhoneNumber = "3111111",
                    UserName = "saul@yopmail.com",
                    Document = "2222",
                    NathivaRole = contentManagerRole
                };

               
                await _usersService.AddUserAsync(user, "12345");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRoles()
        {
            await AdminRoleAsync();
            await ContentManagerAsync();
            await UserManagerAsync();

        }

        private async Task UserManagerAsync()
        {
            bool exists = await _context.NathivaRoles.AnyAsync(r => r.Name == "Gestor de usuarios");

            if (!exists)
            {
                NathivaRole role = new NathivaRole { Name = "Gestor de usuarios" };
                await _context.NathivaRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Usuarios").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { Permission = permission, Role = role });
                }

                await _context.SaveChangesAsync();

            }
        }

        private async Task ContentManagerAsync()
        {
            bool exists = await _context.NathivaRoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                NathivaRole role = new NathivaRole { Name = "Gestor de contenido" };
                await _context.NathivaRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Categorias" || p.Module == "Servicios").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { Permission = permission, Role = role });
                }

                await _context.SaveChangesAsync();

            }
        }

        private async Task AdminRoleAsync()
        {
            //principal: se crea un variable constante en Core Env
            bool exists = await _context.NathivaRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                NathivaRole role = new NathivaRole { Name= Env.SUPER_ADMIN_ROLE_NAME };
                await _context.NathivaRoles.AddAsync(role);
                await _context.SaveChangesAsync();

            }
        }
    }
}
