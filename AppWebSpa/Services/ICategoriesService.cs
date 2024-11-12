using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using AppWebSpa.Request;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;


namespace AppWebSpa.Services
{
    public interface ICategoriesService
    {
        public Task<Response<CategoryService>> CreateAsync(CategoryService model);
        public Task<Response<CategoryService>> DeleteAsync(int categoryId);
        public Task<Response<CategoryService>> EditAsync(CategoryService model);
        public Task<Response<List<CategoryService>>> GetListAsync();
        public Task<Response<CategoryService>> GetOneAsync(int categoryId);
        public Task<Response<CategoryService>> ToggleAsync(ToggleCategoryStatusRequest request);


    }

    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _context;

        public CategoriesService(DataContext context)
        {
            _context = context;

        }

        public async Task<Response<CategoryService>> CreateAsync(CategoryService model)
        {
            try
            {
                CategoryService categoryService = new CategoryService
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                await _context.CategoryServices.AddAsync(categoryService);
                await _context.SaveChangesAsync();
                return ResponseHelper<CategoryService>.MakeResponseSuccess(categoryService, "Categoria creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<CategoryService>> DeleteAsync(int categoryId)
        {
            try
            {
                Response<CategoryService> response = await GetOneAsync(categoryId);

                if (!response.IsSuccess)
                {
                    return response;
                }
                _context.CategoryServices.Remove(response.Result);
                await _context.SaveChangesAsync();
                return ResponseHelper<CategoryService>.MakeResponseSuccess(null, "Categoria eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryService>.MakeResponseFail(ex);
            }

        }

    public async Task<Response<CategoryService>> EditAsync(CategoryService model)
        {
            try
            {
                _context.CategoryServices.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<CategoryService>.MakeResponseSuccess(model, "Categoria editada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryService>.MakeResponseFail(ex);
            }
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

        public async Task<Response<CategoryService>> GetOneAsync(int categoryId)
        {
            try
            {
                CategoryService? categoryService = await _context.CategoryServices.FirstOrDefaultAsync(s => s.CategoryId == categoryId);

                if (categoryService is null)
                {
                    return ResponseHelper<CategoryService>.MakeResponseFail("la Categoria con el Id indicado no existe");
                }

                return ResponseHelper<CategoryService>.MakeResponseSuccess(categoryService);
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<CategoryService>> ToggleAsync(ToggleCategoryStatusRequest request)
        {
            try
            {
                Response<CategoryService> response = await GetOneAsync(request.CategoryId);

                if (!response.IsSuccess)
                {
                    return response;
                }
                CategoryService categoryService = response.Result;

                categoryService.IsHidden = request.Hide;
                _context.CategoryServices.Update(categoryService);
                await _context.SaveChangesAsync();

                return ResponseHelper<CategoryService>.MakeResponseSuccess(null, "Categoria actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryService>.MakeResponseFail(ex);
            }
        }
    }
}
