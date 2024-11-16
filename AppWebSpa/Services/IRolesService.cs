using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Request;
using AppWebSpa.DTOs;
using AppWebSpa.Core.Pagination;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace AppWebSpa.Services
{
        
    public interface IRolesService
    {
        public Task<Response<NathivaRole>> CreateAsync(NathivaRoleDTO dto);
        public Task<Response<PaginationResponse<NathivaRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<NathivaRoleDTO>> GetOneAsync(int id);
        Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<NathivaRole>> CreateAsync(NathivaRoleDTO dto)
        {
            //requiere una transaccion de bases de datos: crear el role, extraer el id del rol y asignar ese id al permiso
            //scope espacio sensible de trabajo para hacer operacion que necesitan un unico entorno

            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    //Creacion del rol
                    NathivaRole role = _converterHelper.ToRole(dto);
                    await _context.NathivaRoles.AddAsync(role);
                    await _context.SaveChangesAsync();

                    // insercion de permiso
                    int roleId = role.Id;

                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        //desserializar: accion de convertir un string a un objeto
                        //Serializar:converti de un objeto a un string
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                    }

                    foreach (int permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }
                    await _context.SaveChangesAsync();

                    //para finalizar una transaccion se hace commit
                    transaction.Commit();
                    return ResponseHelper<NathivaRole>.MakeResponseSuccess(role, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
                }
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

        public async Task<Response<NathivaRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                NathivaRole? role = await _context.NathivaRoles.FirstOrDefaultAsync(s => s.Id == id);

                if (role == null)
                {
                    return ResponseHelper<NathivaRoleDTO>.MakeResponseFail("El rol indicado no existe");
                }

                return ResponseHelper<NathivaRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(role));
            }
            catch (Exception ex)
            {
                return ResponseHelper<NathivaRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync(); 

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);

            }

        }
    }
        
    
}
