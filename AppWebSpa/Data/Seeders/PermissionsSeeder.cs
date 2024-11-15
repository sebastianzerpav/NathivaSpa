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
            List<Permission> permissions = [.. SpaServices(), .. Categories()];

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
                new Permission { Name = "showSpaServices", Description="Ver Servicios", Module="SpaServices"},
                new Permission { Name = "createSpaServices", Description="Crear Servicios", Module="SpaServices"},
                new Permission { Name = "editSpaServices", Description="Editar Servicios", Module="SpaServices"},
                new Permission { Name = "deleteSpaServices", Description="Eliminar Servicios", Module="SpaServices"},
            };
        }

        private List<Permission> Categories()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCategories", Description="Ver Categorias", Module="Categories"},
                new Permission { Name = "createCategories", Description="Crear Categorias", Module="Categories"},
                new Permission { Name = "editCategories", Description="Editar Categorias", Module="Categories"},
                new Permission { Name = "deleteCategories", Description="Eliminar Categorias", Module="Categories"},
            };
        }
    }
}
