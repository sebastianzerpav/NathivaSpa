using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AppWebSpa.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotyfService _notifyService;

        public AccountController(IUserService userService, INotyfService notifyService)
        {
            _userService = userService;
            _notifyService = notifyService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userService.LoginAsync(dto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                _notifyService.Error("Email o contraseña incorrectos");

                return View(dto);
            }

            return View(dto);

        }


        [HttpGet]
        public async Task<IActionResult> Logout() { 
            await _userService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult NotAuthorized() { 
            return View();
        }

    }
}
