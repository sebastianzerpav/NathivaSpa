using Microsoft.AspNetCore.Mvc;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using AppWebSpa.Core;
using AppWebSpa.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using AppWebSpa.Request;
using AppWebSpa.DTOs;




namespace AppWebSpa.Controllers
{
    //[Authorize]
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
                Categories = await _combosHelper.GetComboCategories(),

            };
            return View(dto);
        }

        //Method Create
        [HttpPost]
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
