using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Request;
using AppWebSpa.DTOs;
using static AppWebSpa.Services.IRolesService;
using AppWebSpa.Core.Pagination;

namespace AppWebSpa.Services
{
    public interface IRolesService
    {
        public interface IRolesService
        {
            public Task<Response<RolesForUser>> CreateAsync(RolesForUser model);
            public Task<Response<RolesForUser>> DeleteAsync(int roleId);
            public Task<Response<RolesForUser>> EditAsync(RolesForUser model);
            public Task<Response<PaginationResponse<RolesForUser>>> GetListAsync(PaginationRequest request);
            public Task<Response<RolesForUser>> GetOneAsync(int roleId);
        }

        public class RolesService
        {
            private readonly DataContext _context;

            public RolesService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<RolesForUser>> CreateAsync(RolesForUser model)
            {
                try
                {
                    RolesForUser roles = new RolesForUser
                    {
                        Name = model.Name,
                        State = model.State,
                    };
                    await _context.rolesForUser.AddAsync(roles);
                    await _context.SaveChangesAsync();
                    return ResponseHelper<RolesForUser>.MakeResponseSuccess(roles, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<RolesForUser>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<RolesForUser>> DeleteAsync(int roleId)
            {
                try
                {
                    Response<RolesForUser> response = await GetOneAsync(roleId);

                    if (!response.IsSuccess)
                    {
                        return response;
                    }
                    _context.rolesForUser.Remove(response.Result);
                    await _context.SaveChangesAsync();
                    return ResponseHelper<RolesForUser>.MakeResponseSuccess(null, "Rol eliminado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<RolesForUser>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<RolesForUser>> EditAsync(RolesForUser model)
            {
                try
                {
                    _context.rolesForUser.Update(model);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<RolesForUser>.MakeResponseSuccess(model, "Rol editado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<RolesForUser>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<PaginationResponse<RolesForUser>>> GetListAsync(PaginationRequest request)
            {
                try
                {
                    IQueryable<RolesForUser> query = _context.rolesForUser.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(request.Filter))
                    {
                        query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                    }

                    PagedList<RolesForUser> list = await PagedList<RolesForUser>.ToPagedListAsync(query, request);

                    PaginationResponse<RolesForUser> result = new PaginationResponse<RolesForUser>
                    {
                        List = list,
                        TotalCount = list.TotalCount,
                        RecordsPerPage = list.RecordsPerPage,
                        CurrentPage = list.CurrentPage,
                        TotalPages = list.TotalPages,
                        Filter = request.Filter,
                    };

                    return ResponseHelper<PaginationResponse<RolesForUser>>.MakeResponseSuccess(result, "Roles obtenidos con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<PaginationResponse<RolesForUser>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<RolesForUser>> GetOneAsync(int id)
            {
                try
                {
                    RolesForUser? rolesService = await _context.rolesForUser.FirstOrDefaultAsync(s => s.IdRol == id);

                    if (rolesService is null)
                    {
                        return ResponseHelper<RolesForUser>.MakeResponseFail("El rol con el Id indicado no existe");
                    }

                    return ResponseHelper<RolesForUser>.MakeResponseSuccess(rolesService);
                }
                catch (Exception ex)
                {
                    return ResponseHelper<RolesForUser>.MakeResponseFail(ex);
                }
            }
        }
    }
}
