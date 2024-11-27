using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Models;
using AppWebSpa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppWebSpa.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        [HttpGet]
        [Authorize]
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

            Response<PaginationResponse<Category>> response = await _homeService.GetCategoriesAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromRoute] int id, 
                                                  [FromQuery] int? RecordsPerPage,
                                                  [FromQuery] int? Page,
                                                  [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter

            };

            Response<CategoryDTO> response = await _homeService.GetCategoryAsync(request, id);
            return View(response.Result);

        }

        [HttpGet]
        public async Task<IActionResult> SpaService([FromRoute] int id)
        {
            Response<SpaService> response = await _homeService.GetSpaServiceAsync(id);
            return View(response.Result);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
