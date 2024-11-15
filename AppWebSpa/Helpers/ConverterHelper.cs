using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Helpers
{
    public interface IConverterHelper
    {
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
