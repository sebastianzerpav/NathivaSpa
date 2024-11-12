using AppWebSpa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Helpers
{
    public interface ICombosHelper
    {
        public Task<IEnumerable<SelectListItem>> GetComboCategories();
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
            List<SelectListItem> List = await _context.CategoryServices.Select(s => new SelectListItem
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
    }
}
