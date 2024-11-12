using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;

namespace AppWebSpa.Helpers
{
    public interface IConverterHelper
    {
        public SpaService ToSpaService(SpaServiceDTO dto);
        public Task<SpaServiceDTO> ToSpaServiceDTO(SpaService result);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(ICombosHelper combosHelper)
        {
            _combosHelper = combosHelper;
        }

        public SpaService ToSpaService(SpaServiceDTO dto)
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
    }
}
