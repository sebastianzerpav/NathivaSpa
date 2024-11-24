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
using System.Data;
using System.Collections.Generic;

namespace AppWebSpa.Services
{
        
    public interface IRolesService
    {
        public Task<Response<NathivaRole>> CreateAsync(NathivaRoleDTO dto);
        public Task<Response<NathivaRole>> EditAsync(NathivaRoleDTO dto);
        public Task<Response<IEnumerable<Category>>> GetCategoriesAsync();
        public Task<Response<IEnumerable<CategoryForDTO>>> GetCategoriesByRoleAsync(int id);
        public Task<Response<PaginationResponse<NathivaRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<NathivaRoleDTO>> GetOneAsync(int id);
        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);

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

            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    //Creacion del rol
                    NathivaRole role = _converterHelper.ToRole(dto);
                    await _context.NathivaRoles.AddAsync(role);
                    await _context.SaveChangesAsync();

                    int roleId = role.Id;
                    
                    // insercion de permiso
                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
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

                    // insercion de categorias
                     List<int> categoryIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.CategoryIds))
                    {
                        categoryIds = JsonConvert.DeserializeObject<List<int>>(dto.CategoryIds);
                    }

                    foreach (int categoryId in categoryIds)
                    {
                        RoleCategory roleCategory= new RoleCategory
                        {
                            RoleId = roleId,
                            CategoryId = categoryId
                        };

                        await _context.RoleCategories.AddAsync(roleCategory);
                    }
                    await _context.SaveChangesAsync();
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

        public async Task<Response<NathivaRole>> EditAsync(NathivaRoleDTO dto)
        {
            try
            {
                if(dto.Name== Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<NathivaRole>.MakeResponseFail($"El role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado!!");
                }

                //permisos
                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                //Elimina el permiso antiguo 
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // se inserta nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);  
                }

                //Categorias
                List<int> categoryIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.CategoryIds))
                {
                    categoryIds = JsonConvert.DeserializeObject<List<int>>(dto.CategoryIds);
                }

                //Elimina categorias antiguo 
                List<RoleCategory> oldRoleCategories = await _context.RoleCategories.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RoleCategories.RemoveRange(oldRoleCategories);

                // se inserta nuevas categorias
                foreach (int categoryId in categoryIds)
                {
                    RoleCategory roleCategory = new RoleCategory
                    {
                        RoleId = dto.Id,
                        CategoryId = categoryId
                    };

                    await _context.RoleCategories.AddAsync(roleCategory);
                }

                //actualizacion de rol
                NathivaRole model = _converterHelper.ToRole(dto);
                _context.NathivaRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<NathivaRole>.MakeResponseSuccess(model, "Rol actualizado exitosamente!!");
           

            }
            catch (Exception ex) {
                return ResponseHelper<NathivaRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Category>>> GetCategoriesAsync()
        {
            try
            {
                IEnumerable<Category> categories = await _context.Categories.ToListAsync();

                return ResponseHelper<IEnumerable<Category>>.MakeResponseSuccess(categories);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Category>>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<IEnumerable<CategoryForDTO>>> GetCategoriesByRoleAsync(int id)
        {
            try
            {
                Response<NathivaRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<CategoryForDTO>>.MakeResponseFail(response.Message);
                }

                List<CategoryForDTO> categories = response.Result.Categories;

                return ResponseHelper<IEnumerable<CategoryForDTO>>.MakeResponseSuccess(categories);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<CategoryForDTO>>.MakeResponseFail(ex);
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

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<NathivaRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }
    }
        
    
}
