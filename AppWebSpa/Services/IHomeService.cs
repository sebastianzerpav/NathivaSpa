using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace AppWebSpa.Services
{
    public interface IHomeService
    {
        public Task<Response<PaginationResponse<Category>>> GetCategoriesAsync(PaginationRequest request);
        public Task<Response<CategoryDTO>> GetCategoryAsync(PaginationRequest request, int id);
        public Task<Response<SpaService>> GetSpaServiceAsync(int id);
    }

    public class HomeService : IHomeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IUsersService _userService;

        public HomeService(DataContext context, IHttpContextAccessor httpContextAccesor, IUsersService userService)
        {
            _context = context;
            _httpContextAccesor = httpContextAccesor;
            _userService = userService;
        }

        public async Task<Response<PaginationResponse<Category>>> GetCategoriesAsync(PaginationRequest request)
        {
            try
            {
                ClaimsUser? claimuser = _httpContextAccesor.HttpContext?.User;
                string? userName = claimuser.Identity.Name;
                User user = await _userService.GetUserAsync(userName);

                IQueryable<Category> query = _context.Categories.Include(c => c.RoleCategories)
                                                                .Where(c => !c.IsHidden);
                if (!await _userService.CurrentUserIsSuperAdmin())
                {
                    query = query.Where(c => c.RoleCategories.Any(rc => rc.RoleId == user.NathivaRoleId));
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(c => c.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Category> list = await PagedList<Category>.ToPagedListAsync(query, request);

                PaginationResponse<Category> response = new PaginationResponse<Category>
                {
                    List = list,
                    TotalCount=list.TotalCount,
                    RecordsPerPage=list.RecordsPerPage,
                    CurrentPage=list.CurrentPage,
                    TotalPages=list.TotalPages,
                    Filter=request.Filter,

                };

                return ResponseHelper<PaginationResponse<Category>>.MakeResponseSuccess(response);

            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Category>>.MakeResponseFail(ex);
            }
            

        }

        public async Task<Response<CategoryDTO>> GetCategoryAsync(PaginationRequest request, int id)
        {
            try
            {
                Category? category = await _context.Categories.Include(c => c.RoleCategories)
                                                             .Where(c => !c.IsHidden && c.CategoryId == id)
                                                             .FirstOrDefaultAsync();

                if (category is null)
                {
                    return ResponseHelper<CategoryDTO>.MakeResponseFail($"La categoria con id '{id}'no extiste o esta oculta");
                }

                ClaimsUser? claimuser = _httpContextAccesor.HttpContext?.User;
                string? userName = claimuser.Identity.Name;
                User user = await _userService.GetUserAsync(userName);

                bool isAuthorized = true;

                if (!await _userService.CurrentUserIsSuperAdmin())
                {
                    isAuthorized = category.RoleCategories.Any(rc => rc.RoleId == user.NathivaRoleId);
                }

                if (!isAuthorized)
                {
                    return ResponseHelper<CategoryDTO>.MakeResponseFail("No tiene autorizacion para consultar esta categoria");
                }

                IQueryable<SpaService> query = _context.spaService.Where(s => s.CategoryId == category.CategoryId);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(c => c.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                //reescribir el query para que solo traiga cierts columnas

                query = query.Select(s => new SpaService
                {
                    IdSpaService = s.IdSpaService,
                    Name = s.Name,
                });

                PagedList<SpaService> list = await PagedList<SpaService>.ToPagedListAsync(query, request);

                PaginationResponse<SpaService> paginatedServicesResponse = new PaginationResponse<SpaService>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,

                };

                CategoryDTO dto = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    PaginatedServices = paginatedServicesResponse
                };

                return ResponseHelper<CategoryDTO>.MakeResponseSuccess(dto);

            }
            catch(Exception ex)
            {
                return ResponseHelper<CategoryDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SpaService>> GetSpaServiceAsync(int id)
        {
            try
            {
                SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);

                if (spaService is null) 
                {
                    return ResponseHelper<SpaService>.MakeResponseFail($"El servicio con id '{id}' no existe");
                }

                return ResponseHelper<SpaService>.MakeResponseSuccess(spaService);

            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }
    }
}
