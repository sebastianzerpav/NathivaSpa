
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using AppWebSpa.Core;
using AppWebSpa.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using static System.Collections.Specialized.BitVector32;
using AppWebSpa.Request;
using AppWebSpa.DTOs;



namespace AppWebSpa.Controllers
{
    public class SpaServicesController : Controller
    {
        private readonly ISpaServicesService _spaServicesService;
        public readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;

        public SpaServicesController(ISpaServicesService spaServicesService, INotyfService notifyService, ICombosHelper combosHelper)
        {
            _spaServicesService = spaServicesService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<SpaService>> response = await _spaServicesService.GetListAsync();
            return View(response.Result);
        }

        // View Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            SpaServiceDTO dto = new SpaServiceDTO
            {
                Categories = await _combosHelper.GetComboSections(),

            };
            return View(dto);
        }

        //Method Create
        [HttpPost]
        public async Task<IActionResult> Create(SpaServiceDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    dto.Categories = await _combosHelper.GetComboSections();
                    return View(dto);
                } 

                Response<SpaService> response = await _spaServicesService.CreateAsync(dto);
                
                if(response.IsSuccess)
                {
                    _notifyService.Error(response.Message);
                    dto.Categories = await _combosHelper.GetComboSections();
                    return View(response);
                    
                }

                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View(dto);
            }
        }

        // View Edit
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<SpaService> response = await _spaServicesService.GetOneAsync(id);
            
            if (response.IsSuccess)
            {
                
                return View(response.Result);
            }
            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        //Method Edit
        [HttpPost]
        public async Task<IActionResult> Edit(SpaService spaService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(spaService);
                }

                Response<SpaService> response = await _spaServicesService.EditAsync(spaService);
                
                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(spaService);
            }
        }

        // Delete
        [HttpPost]
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
