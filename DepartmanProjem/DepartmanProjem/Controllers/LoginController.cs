using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepartmanProjem.Data;
using DepartmanProjem.Models;

namespace DepartmanProjem.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            // Eğer kullanıcı zaten giriş yapmışsa ana sayfaya yönlendir
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string kullaniciAd, string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAd) || string.IsNullOrEmpty(sifre))
            {
                TempData["ErrorMessage"] = "Kullanıcı adı ve şifre gereklidir.";
                return View();
            }

            // Veritabanından admin bilgilerini kontrol et
            var admin = await _context.Adminler
                .FirstOrDefaultAsync(a => a.KullaniciAd == kullaniciAd && a.Sifre == sifre);

            if (admin != null)
            {
                // Session'a admin bilgilerini kaydet
                HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
                HttpContext.Session.SetString("KullaniciAd", admin.KullaniciAd);
                
                TempData["SuccessMessage"] = "Giriş başarılı! Hoş geldiniz.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }
        }

        // GET: Logout
        public IActionResult Logout()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index");
        }
    }
}
