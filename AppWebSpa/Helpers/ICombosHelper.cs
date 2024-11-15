using AppWebSpa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Helpers
{
    public interface ICombosHelper
    {
        public Task<IEnumerable<SelectListItem>> GetComboCategories();
        Task<IEnumerable<SelectListItem>> GetComboNathivaRolesAsync();
    }

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCategories()
        {
            List<SelectListItem> List = await _context.Categories.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.CategoryId.ToString(),

            }).ToListAsync();

            List.Insert(0, new SelectListItem
            {
                Text = "[Selecciona una categoria]",
                Value = "0"
            });
            return List;

        }

        public async Task<IEnumerable<SelectListItem>> GetComboNathivaRolesAsync()
        {
        
            List<SelectListItem> List = await _context.NathivaRoles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString(),

            }).ToListAsync();

            List.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });
            return List;
        }
    }
}
