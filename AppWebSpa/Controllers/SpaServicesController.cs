
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using AppWebSpa.Core;
using AppWebSpa.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;



namespace AppWebSpa.Controllers
{
    public class SpaServicesController : Controller
    {
        private readonly ISpaServicesService _spaServicesService;
        public readonly INotyfService _notifyService;

        public SpaServicesController(ISpaServicesService spaServicesService, INotyfService notifyService)
        {
            _spaServicesService = spaServicesService;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<SpaService>> response = await _spaServicesService.GetListAsync();
            return View(response.Result);
        }

        // View Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Method Create
        [HttpPost]
        public async Task<IActionResult> Create(SpaService spaService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(spaService);
                } 

                Response<SpaService> response = await _spaServicesService.CreateAsync(spaService);
                
                if(response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                return View(spaService);
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


    
    }
}
