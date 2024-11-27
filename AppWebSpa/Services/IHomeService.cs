
﻿using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;
using AppWebSpa.Core;
using ClaimUser = System.Security.Claims.ClaimsPrincipal;
using AppWebSpa.Data;
using static System.Collections.Specialized.BitVector32;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Helpers;
using AppWebSpa.DTOs;


namespace AppWebSpa.Services
{
    public interface IHomeService
    {
        public Task<Response<PaginationResponse<Category>>> GetSectionsAsync(PaginationRequest request);
        public Task<Response<CategoryForDTO>> GetSectionAsync(PaginationRequest request, int id);
        public Task<Response<SpaService>> getServiceAsync(int id);
    }

    public class HomeService : IHomeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersService _userService;

        public HomeService(DataContext context, IHttpContextAccessor httpContextAccessor, IUsersService userService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<Response<PaginationResponse<Category>>> GetSectionsAsync(PaginationRequest request)
        {
            try
            {
                ClaimUser? claimuser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimuser.Identity.Name;
                User user = await _userService.GetUserAsync(userName);

                IQueryable<Category> query = _context.Categories.Include(s => s.RoleCategories)
                                                             .Where(s => !s.IsHidden);

                if (!await _userService.CurrentUserIsSuperAdmin())
                {
                    query = query.Where(s => s.RoleCategories.Any(rs => rs.RoleId == user.NathivaRoleId));
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Category> list = await PagedList<Category>.ToPagedListAsync(query, request);

                PaginationResponse<Category> response = new PaginationResponse<Category>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Category>>.MakeResponseSuccess(response);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Category>>.MakeResponseFail(ex);
            }

        }

        public async Task<Response<CategoryForDTO>> GetSectionAsync(PaginationRequest request, int id)
        {
            try
            {
                Category? category = await _context.Categories.Include(s => s.RoleCategories)
                    .Where(s => !s.IsHidden && s.CategoryId == id)
                    .FirstOrDefaultAsync();

                if(category is null)
                {
                    return ResponseHelper<CategoryForDTO>.MakeResponseFail($"La seccion con id '{id}' no existe.");
                }

                ClaimUser? claimuser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimuser.Identity.Name;
                User user = await _userService.GetUserAsync(userName);


                bool isAuthorized = true;
                if (!await _userService.CurrentUserIsSuperAdmin())
                {
                    isAuthorized = category.RoleCategories.Any(rs => rs.RoleId == user.NathivaRoleId);
                }

                if (!isAuthorized)
                {
                    return ResponseHelper<CategoryForDTO>.MakeResponseFail("No esta autorizado para ver esta categoria!!!");
                }

                IQueryable<SpaService> query = _context.spaService.Where( b => b.CategoryId == category.CategoryId);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                query = query.Select(b => new SpaService
                { 
                    IdSpaService = b.IdSpaService,
                    Name = b.Name,
                    Price = b.Price,
                    Description = b.Description
                });

                PagedList<SpaService> list = await PagedList<SpaService>.ToPagedListAsync(query, request);

                PaginationResponse<SpaService> response = new PaginationResponse<SpaService>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                CategoryForDTO dto = new CategoryForDTO
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,

                };

                return ResponseHelper<CategoryForDTO>.MakeResponseSuccess(dto);
            }
            catch (Exception ex)
            {
                return ResponseHelper<CategoryForDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SpaService>> getServiceAsync(int id)
        {
            try
            {
                SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(b => b.IdSpaService == id);
                if (spaService is null)
                {
                    return ResponseHelper<SpaService>.MakeResponseFail($"El servicio con id '{id}' no existe!!!");
                }

                return ResponseHelper<SpaService>.MakeResponseSuccess(spaService);
            } catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

    }
}
