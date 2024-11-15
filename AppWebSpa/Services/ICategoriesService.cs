using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using AppWebSpa.Request;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;


namespace AppWebSpa.Services
{
    public interface ICategoriesService
    {
        public Task<Response<Category>> CreateAsync(Category model);
        public Task<Response<Category>> DeleteAsync(int categoryId);
        public Task<Response<Category>> EditAsync(Category model);
        public Task<Response<PaginationResponse<Category>>> GetListAsync(PaginationRequest request);
        public Task<Response<Category>> GetOneAsync(int categoryId);
        public Task<Response<Category>> ToggleAsync(ToggleCategoryStatusRequest request);


    }

    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _context;

        public CategoriesService(DataContext context)
        {
            _context = context;

        }

        public async Task<Response<Category>> CreateAsync(Category model)
        {
            try
            {
                Category categoryService = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                await _context.Categories.AddAsync(categoryService);
                await _context.SaveChangesAsync();
                return ResponseHelper<Category>.MakeResponseSuccess(categoryService, "Categoria creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Category>> DeleteAsync(int categoryId)
        {
            try
            {
                Response<Category> response = await GetOneAsync(categoryId);

                if (!response.IsSuccess)
                {
                    return response;
                }
                _context.Categories.Remove(response.Result);
                await _context.SaveChangesAsync();
                return ResponseHelper<Category>.MakeResponseSuccess(null, "Categoria eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }

        }

    public async Task<Response<Category>> EditAsync(Category model)
        {
            try
            {
                _context.Categories.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Category>.MakeResponseSuccess(model, "Categoria editada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Category>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Category> query = _context.Categories.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s =>s.Name.ToLower().Contains(request.Filter.ToLower())); 
                }

                PagedList<Category> list =await PagedList<Category>.ToPagedListAsync(query, request);

                PaginationResponse<Category> result = new PaginationResponse<Category>
                {
                    List=list,
                    TotalCount=list.TotalCount,
                    RecordsPerPage=list.RecordsPerPage,
                    CurrentPage =list.CurrentPage,
                    TotalPages=list.TotalPages,
                    Filter=request.Filter,

                };

                return ResponseHelper<PaginationResponse<Category>>.MakeResponseSuccess(result, "Categorias obtenidas con éxito");
            }
            catch (Exception ex) 
            {
                return ResponseHelper<PaginationResponse<Category>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Category>> GetOneAsync(int categoryId)
        {
            try
            {
                Category? categoryService = await _context.Categories.FirstOrDefaultAsync(s => s.CategoryId == categoryId);

                if (categoryService is null)
                {
                    return ResponseHelper<Category>.MakeResponseFail("la Categoria con el Id indicado no existe");
                }

                return ResponseHelper<Category>.MakeResponseSuccess(categoryService);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Category>> ToggleAsync(ToggleCategoryStatusRequest request)
        {
            try
            {
                Response<Category> response = await GetOneAsync(request.CategoryId);

                if (!response.IsSuccess)
                {
                    return response;
                }
                Category categoryService = response.Result;

                categoryService.IsHidden = request.Hide;
                _context.Categories.Update(categoryService);
                await _context.SaveChangesAsync();

                return ResponseHelper<Category>.MakeResponseSuccess(null, "Categoria actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }
    }
}
