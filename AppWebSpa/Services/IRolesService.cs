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
            public Task<Response<NathivaRole>> CreateAsync(NathivaRole model);
            public Task<Response<NathivaRole>> DeleteAsync(int roleId);
            public Task<Response<NathivaRole>> EditAsync(NathivaRole model);
            public Task<Response<PaginationResponse<NathivaRole>>> GetListAsync(PaginationRequest request);
            public Task<Response<NathivaRole>> GetOneAsync(int roleId);
        }

        public class RolesService
        {
            private readonly DataContext _context;

            public RolesService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<NathivaRole>> CreateAsync(NathivaRole model)
            {
                try
                {
                    NathivaRole roles = new NathivaRole
                    {
                        Name = model.Name,
                        State = model.State,
                    };
                    await _context.NathivaRoles.AddAsync(roles);
                    await _context.SaveChangesAsync();
                    return ResponseHelper<NathivaRole>.MakeResponseSuccess(roles, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<NathivaRole>> DeleteAsync(int roleId)
            {
                try
                {
                    Response<NathivaRole> response = await GetOneAsync(roleId);

                    if (!response.IsSuccess)
                    {
                        return response;
                    }
                    _context.NathivaRoles.Remove(response.Result);
                    await _context.SaveChangesAsync();
                    return ResponseHelper<NathivaRole>.MakeResponseSuccess(null, "Rol eliminado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<NathivaRole>> EditAsync(NathivaRole model)
            {
                try
                {
                    _context.NathivaRoles.Update(model);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<NathivaRole>.MakeResponseSuccess(model, "Rol editado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<PaginationResponse<NathivaRole>>> GetListAsync(PaginationRequest request)
            {
                try
                {
                    IQueryable<NathivaRole> query = _context.NathivaRoles.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(request.Filter))
                    {
                        query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                    }

                    PagedList<NathivaRole> list = await PagedList<NathivaRole>.ToPagedListAsync(query, request);

                    PaginationResponse<NathivaRole> result = new PaginationResponse<NathivaRole>
                    {
                        List = list,
                        TotalCount = list.TotalCount,
                        RecordsPerPage = list.RecordsPerPage,
                        CurrentPage = list.CurrentPage,
                        TotalPages = list.TotalPages,
                        Filter = request.Filter,
                    };

                    return ResponseHelper<PaginationResponse<NathivaRole>>.MakeResponseSuccess(result, "Roles obtenidos con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<PaginationResponse<NathivaRole>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<NathivaRole>> GetOneAsync(int id)
            {
                try
                {
                    NathivaRole? rolesService = await _context.NathivaRoles.FirstOrDefaultAsync(s => s.Id == id);

                    if (rolesService is null)
                    {
                        return ResponseHelper<NathivaRole>.MakeResponseFail("El rol con el Id indicado no existe");
                    }

                    return ResponseHelper<NathivaRole>.MakeResponseSuccess(rolesService);
                }
                catch (Exception ex)
                {
                    return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
                }
            }
        }
    }
}
