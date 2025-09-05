using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DepartmanProjem.Data;
using DepartmanProjem.Models;
using DepartmanProjem.Filters;

namespace DepartmanProjem.Controllers
{
    [LoginRequired]
    public class PersonelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Personel
        public async Task<IActionResult> Index()
        {
            var personeller = await _context.Personeller
                .Include(p => p.Birim)
                .ToListAsync();
            return View(personeller);
        }

        // GET: Personel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller
                .Include(p => p.Birim)
                .FirstOrDefaultAsync(m => m.PersonelId == id);

            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // GET: Personel/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BirimId"] = new SelectList(await _context.Birimler.ToListAsync(), "BirimId", "BirimAd");
            return View();
        }

        // POST: Personel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonelId,Ad,Soyad,BirimId")] Personel personel)
        {
            // Debug için gelen verileri logla
            Console.WriteLine($"Gelen Personel: Ad={personel.Ad}, Soyad={personel.Soyad}, BirimId={personel.BirimId}");
            
            // ModelState'i temizle ve manuel validation yap
            ModelState.Clear();
            
            // Ad kontrolü
            if (string.IsNullOrWhiteSpace(personel.Ad))
            {
                ModelState.AddModelError("Ad", "Ad alanı zorunludur.");
            }
            else if (personel.Ad.Length > 50)
            {
                ModelState.AddModelError("Ad", "Ad en fazla 50 karakter olabilir.");
            }
            
            // Soyad kontrolü
            if (string.IsNullOrWhiteSpace(personel.Soyad))
            {
                ModelState.AddModelError("Soyad", "Soyad alanı zorunludur.");
            }
            else if (personel.Soyad.Length > 50)
            {
                ModelState.AddModelError("Soyad", "Soyad en fazla 50 karakter olabilir.");
            }
            
            // Birim kontrolü
            if (personel.BirimId == 0)
            {
                ModelState.AddModelError("BirimId", "Birim seçimi zorunludur.");
            }
            else
            {
                // Seçilen birimin veritabanında var olup olmadığını kontrol et
                var birimExists = await _context.Birimler.AnyAsync(b => b.BirimId == personel.BirimId);
                if (!birimExists)
                {
                    ModelState.AddModelError("BirimId", "Seçilen birim bulunamadı.");
                }
            }
            
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını detaylı logla
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                        TempData["ErrorMessage"] = error.ErrorMessage;
                    }
                }
                
                ViewData["BirimId"] = new SelectList(await _context.Birimler.ToListAsync(), "BirimId", "BirimAd", personel.BirimId);
                return View(personel);
            }

            _context.Add(personel);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Personel başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Personel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            ViewData["BirimId"] = new SelectList(await _context.Birimler.ToListAsync(), "BirimId", "BirimAd", personel.BirimId);
            return View(personel);
        }

        // POST: Personel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonelId,Ad,Soyad,BirimId")] Personel personel)
        {
            if (id != personel.PersonelId)
            {
                return NotFound();
            }

            // Debug için gelen verileri logla
            Console.WriteLine($"Güncellenen Personel: ID={personel.PersonelId}, Ad={personel.Ad}, Soyad={personel.Soyad}, BirimId={personel.BirimId}");
            
            // ModelState'i temizle ve manuel validation yap
            ModelState.Clear();
            
            // Ad kontrolü
            if (string.IsNullOrWhiteSpace(personel.Ad))
            {
                ModelState.AddModelError("Ad", "Ad alanı zorunludur.");
            }
            else if (personel.Ad.Length > 50)
            {
                ModelState.AddModelError("Ad", "Ad en fazla 50 karakter olabilir.");
            }
            
            // Soyad kontrolü
            if (string.IsNullOrWhiteSpace(personel.Soyad))
            {
                ModelState.AddModelError("Soyad", "Soyad alanı zorunludur.");
            }
            else if (personel.Soyad.Length > 50)
            {
                ModelState.AddModelError("Soyad", "Soyad en fazla 50 karakter olabilir.");
            }
            
            // Birim kontrolü
            if (personel.BirimId == 0)
            {
                ModelState.AddModelError("BirimId", "Birim seçimi zorunludur.");
            }
            else
            {
                // Seçilen birimin veritabanında var olup olmadığını kontrol et
                var birimExists = await _context.Birimler.AnyAsync(b => b.BirimId == personel.BirimId);
                if (!birimExists)
                {
                    ModelState.AddModelError("BirimId", "Seçilen birim bulunamadı.");
                }
            }
            
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını detaylı logla
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                        TempData["ErrorMessage"] = error.ErrorMessage;
                    }
                }
                
                ViewData["BirimId"] = new SelectList(await _context.Birimler.ToListAsync(), "BirimId", "BirimAd", personel.BirimId);
                return View(personel);
            }

            try
            {
                _context.Update(personel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Personel başarıyla güncellendi.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonelExists(personel.PersonelId))
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

        // GET: Personel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller
                .Include(p => p.Birim)
                .FirstOrDefaultAsync(m => m.PersonelId == id);
            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // POST: Personel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel != null)
            {
                _context.Personeller.Remove(personel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Personel başarıyla silindi.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PersonelExists(int id)
        {
            return _context.Personeller.Any(e => e.PersonelId == id);
        }
    }
}
