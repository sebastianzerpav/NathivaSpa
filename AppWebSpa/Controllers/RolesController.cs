using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AppWebSpa.Core.Attributes;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Core;
using AppWebSpa.DTOs;



namespace AppWebSpa.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notifyService;

        public RolesController(IRolesService rolesService, INotyfService notifyService)
        {
            _rolesService = rolesService;
            _notifyService = notifyService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
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

            Response<PaginationResponse<NathivaRole>> response = await _rolesService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            //1. Traer los permisos
            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            NathivaRoleDTO dto = new NathivaRoleDTO
            {
                Permissions=response.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList()
                //Select esta formateando la lista estatica
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(NathivaRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los erreres de validacion");

                Response<IEnumerable<Permission>> response1 = await _rolesService.GetPermissionsAsync();

                dto.Permissions = response1.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                return View(dto);
            }

            Response<NathivaRole> createResponse = await _rolesService.CreateAsync(dto);

            if (createResponse.IsSuccess)
            {
                _notifyService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));

            }
            _notifyService.Error(createResponse.Message);

            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();
            dto.Permissions = response.Result.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
            }).ToList();

            return View(dto);
        }

    }
}
