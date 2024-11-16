using AppWebSpa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. SpaServices(), .. Categories(), .. Users(), .. Roles()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name
                                                                 && p.Module == permission.Module);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }

            }
            await _context.SaveChangesAsync();
        }

        private List<Permission> SpaServices()
        {
            return new List<Permission>
            {
                new Permission { Name = "showSpaServices", Description="Ver Servicios", Module="Servicios"},
                new Permission { Name = "createSpaServices", Description="Crear Servicios", Module="Servicios"},
                new Permission { Name = "updateSpaServices", Description="Editar Servicios", Module="Servicios"},
                new Permission { Name = "deleteSpaServices", Description="Eliminar Servicios", Module="Servicios"},
            };
        }

        private List<Permission> Categories()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCategories", Description="Ver Categorias", Module="Categorias"},
                new Permission { Name = "createCategories", Description="Crear Categorias", Module="Categorias"},
                new Permission { Name = "updateCategories", Description="Editar Categorias", Module="Categorias"},
                new Permission { Name = "deleteCategories", Description="Eliminar Categorias", Module="Categorias"},
            };
        }

        private List<Permission> Users()
        {
            return new List<Permission>
            {
                new Permission { Name = "showUsers", Description="Ver Usuarios", Module="Usuarios"},
                new Permission { Name = "createUsers", Description="Crear Usuarios", Module="Usuarios"},
                new Permission { Name = "updateUsers", Description="Editar Usuarios", Module="Usuarios"},
                new Permission { Name = "deleteUsers", Description="Eliminar Usuarios", Module="Usuarios"},
            };
        }

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Name = "showRoles", Description="Ver Roles", Module="Roles"},
                new Permission { Name = "createRoles", Description="Crear Roles", Module="Roles"},
                new Permission { Name = "updateRoles", Description="Editar Roles", Module="Roles"},
                new Permission { Name = "deleteRoles", Description="Eliminar Roles", Module="Roles"},
            };
        }
    }
}
