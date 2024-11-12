﻿using AppWebSpa.Core;
using AppWebSpa.Data.Entities;
using AppWebSpa.Request;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace AppWebSpa.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            Response<List<CategoryService>> response =await _categoriesService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryService categoryService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(categoryService);
                }

                Response<CategoryService> response = await _categoriesService.CreateAsync(categoryService);

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
            Response<CategoryService> response = await _categoriesService.GetOneAsync(id);
            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryService categoryService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    return View(categoryService);
                }

                Response<CategoryService> response = await _categoriesService.EditAsync(categoryService);

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
            Response<CategoryService> response = await _categoriesService.DeleteAsync(id);
            
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

            Response<CategoryService> response = await _categoriesService.ToggleAsync(request);

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
