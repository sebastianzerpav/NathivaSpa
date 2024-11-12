using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;


namespace AppWebSpa.Services
{
    public interface ICategoriesService
    {
        public Task<Response<List<CategoryService>>> GetListAsync();

    }

    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _context;

        public CategoriesService(DataContext context)
        {
            _context = context;

        }

        public async Task<Response<List<CategoryService>>> GetListAsync()
        {
            try
            {
                List<CategoryService> list = await _context.CategoryServices.ToListAsync();

                return ResponseHelper<List<CategoryService>>.MakeResponseSuccess(list);

            }
            catch (Exception ex) 
            {
                return ResponseHelper<List<CategoryService>>.MakeResponseFail(ex);
            }
        }
    }
}
