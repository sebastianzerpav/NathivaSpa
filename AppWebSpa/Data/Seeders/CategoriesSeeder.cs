using AppWebSpa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data.Seeders
{
    public class CategoriesSeeder
    {
        private readonly DataContext _context;

        public CategoriesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Category> categories = new List<Category>
            {
                new Category {Name= "Uñas" , Description="Cuidados de las uñas"},
                new Category {Name= "Facial" , Description="Cuidados del rostro"},
                new Category {Name= "Cabello" , Description="Cuidados del cabello"},
            };

            foreach (Category category in categories)
            {
                bool exists = await _context.Categories.AnyAsync(c => c.Name == category.Name);

                if (!exists)
                {
                    await _context.Categories.AddAsync(category);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
