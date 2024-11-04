
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Data;
using AppWebSpa.Models;

namespace AppWebSpa.Controllers
{
    public class SpaServicesController : Controller
    {
        private readonly AppDbContext _context;

        public SpaServicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SpaService> services = await _context.spaService.ToListAsync();
            return View(services);
        }

        // View specific Service detail
        [HttpGet]
        public async Task<IActionResult> ServiceDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);
            if (spaService == null)
            {
                return NotFound();
            }

            return View(spaService);
        }

        // View Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Method Create
        [HttpPost]
        public async Task<IActionResult> Create(SpaService spaService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(spaService);
                }
                await _context.spaService.AddAsync(spaService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // View Edit
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);
            if (spaService == null)
            {
                return NotFound();
            }
            return View(spaService);
        }

        //Method Edit
        [HttpPost]
        public async Task<IActionResult> Edit(SpaService spaService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(spaService);
                }
                _context.spaService.Update(spaService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // view Delete
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute]int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);
            if (spaService == null)
            {
                return NotFound();
            }

            return View(spaService);
        }

        // Method Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> AcceptDelete(int id)
        {
            SpaService? spaService = await _context.spaService.FirstOrDefaultAsync(s => s.IdSpaService == id);
            if (spaService != null)
            {
                _context.spaService.Remove(spaService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
