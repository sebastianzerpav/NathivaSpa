using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Helpers;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersService _userService;
        private readonly IConverterHelper _converterHelper;
        public UsersController(IUsersService userService, ICombosHelper combosHelper, INotyfService notifyService, IConverterHelper converterHelper)
        {
            _userService = userService;
            _combosHelper = combosHelper;
            _notifyService = notifyService;
            _converterHelper = converterHelper;
        }

        //View Index with list of Users
        [HttpGet]
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

            Response<PaginationResponse<User>> response = await _userService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserDTO dto = new UserDTO
            {
                NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync(),
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    dto.NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync();
                    return View(dto);
                }

                Response<User> response = await _userService.CreateAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                dto.NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync();
                return View(dto);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserDTO dto = await _converterHelper.ToUserDTOAsync(user, false);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Metodo para evitar secuestros de seccion, genera tokens unicos
        public async Task<IActionResult> Edit(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validacion");
                dto.NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync();
                return View(dto);
            }

            Response<User> response = await _userService.UpdateUserAsync(dto);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            dto.NathivaRoles = await _combosHelper.GetComboNathivaRolesAsync();
            return View(dto);


        }
    }
}
