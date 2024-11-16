using Microsoft.AspNetCore.Mvc;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using AppWebSpa.Core;
using AppWebSpa.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using AppWebSpa.Request;
using AppWebSpa.DTOs;
using AppWebSpa.Core.Pagination;
using Microsoft.AspNetCore.Authorization;
using AppWebSpa.Core.Attributes;




namespace AppWebSpa.Controllers
{
    [Authorize]
    public class SpaServicesController : Controller
    {
        private readonly ISpaServicesService _spaServicesService;
        public readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public SpaServicesController(ISpaServicesService spaServicesService, INotyfService notifyService, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _spaServicesService = spaServicesService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }
        [HttpGet]
        [CustomAuthorize(permission: "showSpaServices", module: "Servicios")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                              [FromQuery] int? Page,
                                              [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter

            };

            Response<PaginationResponse<SpaService>> response = await _spaServicesService.GetListAsync(request);
            return View(response.Result);
        }
      

        // View Create
        [HttpGet]
        [CustomAuthorize(permission: "createSpaServices", module: "Servicios")]
        public async Task<IActionResult> Create()
        {
            SpaServiceDTO dto = new SpaServiceDTO
            {
                Categories = await _combosHelper.GetComboCategories(),

            };
            return View(dto);
        }

        //Method Create
        [HttpPost]
        [CustomAuthorize(permission: "createSpaServices", module: "Servicios")]
        public async Task<IActionResult> Create(SpaServiceDTO dto)
        {
            
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validacion");
                dto.Categories = await _combosHelper.GetComboCategories();
                return View(dto);
            } 

            Response<SpaService> response = await _spaServicesService.CreateAsync(dto);
                
            if(!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.Categories = await _combosHelper.GetComboCategories();
                return View(dto);
                    
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));

            
        }

        // View Edit
        [HttpGet]
        [CustomAuthorize(permission: "updateSpaServices", module: "Servicios")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<SpaService> response = await _spaServicesService.GetOneAsync(id);
            
            if (response.IsSuccess)
            {
                SpaServiceDTO dto =await _converterHelper.ToSpaServiceDTO(response.Result);
                dto.Categories=await _combosHelper.GetComboCategories();
                return View(dto);
            }
            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        //Method Edit
        [HttpPost]
        [CustomAuthorize(permission: "updateSpaServices", module: "Servicios")]
        public async Task<IActionResult> Edit(SpaServiceDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(dto);
                }

                Response<SpaService> response = await _spaServicesService.EditAsync(dto);
                
                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.Categories= await _combosHelper.GetComboCategories();
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                dto.Categories = await _combosHelper.GetComboCategories();
                return View(dto);
            }
        }

        // Delete
        [HttpPost]
        [CustomAuthorize(permission: "deleteSpaServices", module: "Servicios")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<SpaService> response = await _spaServicesService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                
            }
            else
            {
                _notifyService.Error(response.Message);

            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateSpaServices", module: "Servicios")]
        public async Task<IActionResult> Toggle(int SpaServiceId, bool Hide)
        {
            ToggleSpaServiceStatusRequest request = new ToggleSpaServiceStatusRequest
            {
                Hide = Hide,
                SpaServiceId = SpaServiceId
            };

            Response<SpaService> response = await _spaServicesService.ToggleAsync(request);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);

            }
            else
            {
                _notifyService.Error(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }



    }
}
