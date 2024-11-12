using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AppWebSpa.Request;
using static System.Collections.Specialized.BitVector32;
using AppWebSpa.DTOs;
//using static System.Collections.Specialized.BitVector32;

namespace AppWebSpa.Services
{
    public interface ISpaServicesService
    {
        public Task<Response<SpaService>> CreateAsync(SpaServiceDTO dto);
        public Task<Response<SpaService>> DeleteAsync(int id);
        public Task<Response<SpaService>> EditAsync(SpaService model);
        public Task<Response<List<SpaService>>> GetListAsync();
        public Task<Response<SpaService>> GetOneAsync(int id);
        public Task<Response<SpaService>> ToggleAsync(ToggleSpaServiceStatusRequest request);

    }

    public class SpaServicesService : ISpaServicesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public SpaServicesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<SpaService>> CreateAsync(SpaServiceDTO dto)
        {
            try
            {
                SpaService spaservice = _converterHelper.ToSpaService(dto);


                
                await _context.spaService.AddAsync(spaservice);
                await _context.SaveChangesAsync();

                return ResponseHelper<SpaService>.MakeResponseSuccess(spaservice, "Servicio creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SpaService>> DeleteAsync(int id)
        {
            try
            {
                Response<SpaService> response = await GetOneAsync(id);
                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.spaService.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<SpaService>.MakeResponseSuccess(null, "Seccion eliminada con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SpaService>> EditAsync(SpaService model)
        {
            try
            {
                _context.spaService.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<SpaService>.MakeResponseSuccess(model, "Servicio editado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<SpaService>>> GetListAsync()
        {
            try
            {
                List<SpaService> spaservices = await _context.spaService.Include(b => b.CategoryService).ToListAsync();

                return ResponseHelper<List<SpaService>>.MakeResponseSuccess(spaservices);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<SpaService>>.MakeResponseFail(ex);
            }

        }

        public async Task<Response<SpaService>> GetOneAsync(int id)
        {
            try
            {
                SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);

                if (spaService == null)
                {
                    return ResponseHelper<SpaService>.MakeResponseFail("El servicio indicado no existe");
                }

                return ResponseHelper<SpaService>.MakeResponseSuccess(spaService);
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SpaService>> ToggleAsync(ToggleSpaServiceStatusRequest request)
        {
            try
            {
                Response<SpaService> response = await GetOneAsync(request.SpaServiceId);

                if (!response.IsSuccess)
                {
                    return response;
                }
                SpaService spaService = response.Result;

                spaService.IsHidden = request.Hide;
                _context.spaService.Update(spaService);
                await _context.SaveChangesAsync();

                return ResponseHelper<SpaService>.MakeResponseSuccess(null, "Servicio actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }
    }
}
