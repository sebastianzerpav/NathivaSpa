using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;
using AppWebSpa.Request;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace AppWebSpa.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly INotyfService _notifyService;

        public CategoriesController(ICategoriesService categoriesService, INotyfService notyfService)
        {
            _categoriesService = categoriesService;
            _notifyService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage= RecordsPerPage ?? 15,
                Page= Page ?? 1,
                Filter= Filter
                
            };

            Response<PaginationResponse<Category>> response =await _categoriesService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category categoryService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(categoryService);
                }

                Response<Category> response = await _categoriesService.CreateAsync(categoryService);

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
                return View(categoryService);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Category> response = await _categoriesService.GetOneAsync(id);
            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category categoryService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(categoryService);
                }

                Response<Category> response = await _categoriesService.EditAsync(categoryService);

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
                return View(categoryService);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Category> response = await _categoriesService.DeleteAsync(id);
            
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
        public async Task<IActionResult> Toggle(int CategoryId, bool Hide)
        {
            ToggleCategoryStatusRequest request = new ToggleCategoryStatusRequest
            {
                Hide = Hide,
                CategoryId = CategoryId
            };

            Response<Category> response = await _categoriesService.ToggleAsync(request);

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
