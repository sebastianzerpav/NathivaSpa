using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;

namespace AppWebSpa.Helpers
{
    public interface IConverterHelper
    {
        public SpaService ToSpaService(SpaServiceDTO dto);
    }

    public class ConverterHelper : IConverterHelper
    {
        public SpaService ToSpaService(SpaServiceDTO dto)
        {
            return new SpaService
            {
                IdSpaService=dto.IdSpaService,
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price,
                RegistrationDateTime=dto.RegistrationDateTime,
                IsHidden=dto.IsHidden,
                CategoryId=dto.CategoryId,             

            };
        }
    }
}
