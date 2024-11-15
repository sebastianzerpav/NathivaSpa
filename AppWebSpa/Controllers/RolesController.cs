using AppWebSpa.Data.Entities;
using AppWebSpa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Models;

namespace AppWebSpa.Controllers
{
    public class RolesController : Controller
    {
        private readonly DataContext _context;

        public RolesController(DataContext context)
        {
            _context = context;
        }

        // View Index with list of Roles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<NathivaRole> roles = await _context.NathivaRoles.ToListAsync();
            return View(roles);
        }

        // View specific role details
        [HttpGet]
        public async Task<IActionResult> RolesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                NathivaRole? role = await _context.NathivaRoles.FirstOrDefaultAsync(r => r.Id == id);
                if (role == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(role);
                }
            }
        }

        // View create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NathivaRole rol)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(rol);
                }
                await _context.NathivaRoles.AddAsync(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // View edit specific role
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                NathivaRole? role = await _context.NathivaRoles.FirstOrDefaultAsync(r => r.Id == id);
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(NathivaRole rol)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(rol);
                }
                _context.NathivaRoles.Update(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // View delete specific role
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                NathivaRole? role = await _context.NathivaRoles.FirstOrDefaultAsync(r => r.Id == id);
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }
        }

        // Method delete role
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> EffectiveDelete(int id)
        {
            NathivaRole? role = await _context.NathivaRoles.FirstOrDefaultAsync(r => r.Id == id);
            if (role != null)
            {
                _context.NathivaRoles.Remove(role);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Vista para asignar el rol
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var users = await _context.User.ToListAsync();
            var roles = await _context.NathivaRoles.ToListAsync();

            var model = new AssignRoleViewModel
            {
                Users = users,
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            user.NathivaRoleId = roleId;
            _context.Entry(user).Property(u => u.NathivaRoleId).IsModified = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
