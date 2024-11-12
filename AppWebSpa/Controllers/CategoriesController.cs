using AppWebSpa.Core;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppWebSpa.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService) 
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            Response<List<CategoryService>> response =await _categoriesService.GetListAsync();
            return View(response.Result);
        }
    }
    
}
