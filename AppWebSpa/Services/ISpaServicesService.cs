using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Services
{
    public interface ISpaServicesService
    {
        public Task<Response<List<SpaService>>> GetListAsync();
    }

    public class SpaServicesService : ISpaServicesService
    {
        private readonly DataContext _context;

        public SpaServicesService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SpaService>>> GetListAsync()
        {
            try
            {
                List<SpaService> spaservices = await _context.spaService.ToListAsync();

                return ResponseHelper<List<SpaService>>.MakeResponseSuccess(spaservices);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<SpaService>>.MakeResponseFail(ex);
            }


        }
    }
}
