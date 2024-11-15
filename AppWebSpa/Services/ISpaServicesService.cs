using AppWebSpa.Core;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Operations;
using AppWebSpa.Request;
using AppWebSpa.DTOs;
using AppWebSpa.Core.Pagination;


namespace AppWebSpa.Services
{
    public interface ISpaServicesService
    {
        public Task<Response<SpaService>> CreateAsync(SpaServiceDTO dto);
        public Task<Response<SpaService>> DeleteAsync(int id);
        public Task<Response<SpaService>> EditAsync(SpaServiceDTO dto);
        public Task<Response<PaginationResponse<SpaService>>> GetListAsync(PaginationRequest request);
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
                SpaService spaservice = await _converterHelper.ToSpaService(dto);

                //spaservice.CategoryService = await _context.Categories.FirstAsync(c => c.CategoryId == dto.CategoryId);

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

        public async Task<Response<SpaService>> EditAsync(SpaServiceDTO dto)
        {
            try
            {
                SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == dto.IdSpaService);
                
                if(spaService is null)
                {
                    return ResponseHelper<SpaService>.MakeResponseFail($"No existe servicio con id '{dto.IdSpaService}'");
                }

                spaService.Name = dto.Name;
                spaService.Description = dto.Description;
                spaService.Price = dto.Price;
                spaService.CategoryId = dto.CategoryId;
                spaService.CategoryService = await _context.Categories.FirstAsync(c => c.CategoryId == dto.CategoryId);


                _context.spaService.Update(spaService);
                await _context.SaveChangesAsync();

                return ResponseHelper<SpaService>.MakeResponseSuccess(spaService, "Servicio editado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SpaService>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<SpaService>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<SpaService> query = _context.spaService.AsQueryable().Include(b =>b.CategoryService);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<SpaService> list = await PagedList<SpaService>.ToPagedListAsync(query, request);

                PaginationResponse<SpaService> result = new PaginationResponse<SpaService>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,

                };
                return ResponseHelper<PaginationResponse<SpaService>>.MakeResponseSuccess(result, "Servicios obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<SpaService>>.MakeResponseFail(ex);
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
