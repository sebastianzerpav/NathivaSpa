using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        IUserService _userService;
        public UsersController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //View Index with list of Users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<User> Users = await _context.User.ToListAsync();
            return View(Users);
        }

        //View specific user details
        [HttpGet]
        public async Task<IActionResult> UserDetails(string? id)
        {
            if (id == null)
            {
                return NotFound(); //Podemos personalizar error
            }
            else
            {
                User? user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) { return NotFound(); } //x2
                else { return View(user); }
            }
        }

        // View create
        public IActionResult Create()
        {
            return View();
        }

        // Method Create - Register user
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                await _context.User.AddAsync(user);
                await _userService.AddUserAsync(user, "111");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            { return RedirectToAction(nameof(Index)); }
        }

        // View Edit
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                User? user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
        }

        // Method Edit user 
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            { return RedirectToAction(nameof(Index)); }
        }

        // View Delete specific user
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                User? user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                { return NotFound(); }

                return View(user);
            }
        }

        // Method Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> EffectiveDelete(string id)
        {
            User? user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
