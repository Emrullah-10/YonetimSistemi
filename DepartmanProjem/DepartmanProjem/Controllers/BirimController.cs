using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepartmanProjem.Data;
using DepartmanProjem.Models;
using DepartmanProjem.Filters;

namespace DepartmanProjem.Controllers
{
    [LoginRequired]
    public class BirimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BirimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Birim
        public async Task<IActionResult> Index()
        {
            var birimler = await _context.Birimler.ToListAsync();
            return View(birimler);
        }

        // GET: Birim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birim = await _context.Birimler
                .Include(b => b.Personeller)
                .FirstOrDefaultAsync(m => m.BirimId == id);

            if (birim == null)
            {
                return NotFound();
            }

            return View(birim);
        }

        // GET: Birim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BirimId,BirimAd")] Birim birim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(birim);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Birim başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(birim);
        }

        // GET: Birim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birim = await _context.Birimler.FindAsync(id);
            if (birim == null)
            {
                return NotFound();
            }
            return View(birim);
        }

        // POST: Birim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BirimId,BirimAd")] Birim birim)
        {
            if (id != birim.BirimId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(birim);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Birim başarıyla güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirimExists(birim.BirimId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(birim);
        }

        // GET: Birim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birim = await _context.Birimler
                .FirstOrDefaultAsync(m => m.BirimId == id);
            if (birim == null)
            {
                return NotFound();
            }

            return View(birim);
        }

        // POST: Birim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birim = await _context.Birimler.FindAsync(id);
            if (birim != null)
            {
                _context.Birimler.Remove(birim);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Birim başarıyla silindi.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BirimExists(int id)
        {
            return _context.Birimler.Any(e => e.BirimId == id);
        }
    }
}
