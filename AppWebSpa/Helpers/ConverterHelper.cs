using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Helpers
{
    public interface IConverterHelper
    {
        public NathivaRole ToRole(NathivaRoleDTO dto);
        public Task<NathivaRoleDTO> ToRoleDTOAsync(NathivaRole role);
        public Task<SpaService> ToSpaService(SpaServiceDTO dto);
        public Task<SpaServiceDTO> ToSpaServiceDTO(SpaService result);
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _context;

        public ConverterHelper(ICombosHelper combosHelper, DataContext context)
        {
            _combosHelper = combosHelper;
            _context = context;
        }

        public NathivaRole ToRole(NathivaRoleDTO dto)
        {
            return new NathivaRole
            {
                Id = dto.Id,
                Name = dto.Name,

            };
        }

        public async Task<NathivaRoleDTO> ToRoleDTOAsync(NathivaRole role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,  
                Description = p.Description,
                Module = p.Module,
                Selected=_context.RolePermissions.Any(rp => rp.PermissionId==p.Id && rp.RoleId==role.Id)

            }).ToListAsync();

            List<CategoryForDTO> categories = await _context.Categories.Select(p => new CategoryForDTO
            {
                CategoryId = p.CategoryId,
                Name = p.Name,
                Description=p.Description,
                Selected = _context.RoleCategories.Any(rs => rs.CategoryId == p.CategoryId && rs.RoleId == role.Id)

            }).ToListAsync();


            return new NathivaRoleDTO
            {
                Id=role.Id,
                Name=role.Name, 
                Permissions = permissions,
                Categories = categories
            };
        }

        public async Task<SpaService> ToSpaService(SpaServiceDTO dto)
        {
            return new SpaService
            {
                IdSpaService=dto.IdSpaService,
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price,
                RegistrationDateTime = dto.RegistrationDateTime,
                IsHidden =dto.IsHidden,
                CategoryId=dto.CategoryId,
                CategoryService = await _context.Categories.FirstAsync(c => c.CategoryId == dto.CategoryId),

            };
        }

        public async Task<SpaServiceDTO> ToSpaServiceDTO(SpaService result)
        {
            return new SpaServiceDTO
            {
                IdSpaService = result.IdSpaService,
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                RegistrationDateTime = result.RegistrationDateTime,
                IsHidden = result.IsHidden,
                CategoryId = result.CategoryId,
                Categories = await _combosHelper.GetComboCategories()
            };
        }

        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id.ToString(),
                Document = dto.Document,
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                UserName = dto.Email,
                NathivaRoleId = dto.NathivaRoleId,
                PhoneNumber = dto.PhoneNumber,

            };
        }

        public async Task<UserDTO> ToUserDTOAsync(User user, bool isNew= true)
        {
            return new UserDTO
            {
                Id = isNew ? Guid.NewGuid() : Guid.Parse(user.Id),
                Document = user.Document,
                Name = user.Name,
                Email = user.Email,
                NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync(),
                NathivaRoleId=user.NathivaRoleId,
                PhoneNumber=user.PhoneNumber

            };
        }
    }
}
