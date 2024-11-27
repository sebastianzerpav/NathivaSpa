using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;
using AppWebSpa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AppWebSpa.Core;
using AppWebSpa.Services;
using AppWebSpa.DTOs;


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

            Response<PaginationResponse<Category>> response = await _homeService.GetSectionsAsync(request);
            return View(response.Result);
        }

        public async Task<IActionResult> Categories([FromRoute] int id,
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

            Response<CategoryForDTO> response = await _homeService.GetSectionAsync(request, id);
            return View(response.Result);
        }

        [HttpGet]

        public async Task<IActionResult> Services([FromRoute] int id)
        {
            Response<SpaService> response = await _homeService.getServiceAsync(id);
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
