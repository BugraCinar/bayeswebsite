using Microsoft.AspNetCore.Mvc;
using bayessoft.Data;
using bayessoft.Models;
using BCrypt.Net;
using System.Linq;
using Microsoft.AspNetCore.Http; 

namespace bayessoft.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/admin")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/admin")]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username);
            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
            {
                
                HttpContext.Session.SetString("admin_giris", "ok");
                return RedirectToAction("Panel");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre yanlış.";
            return View();
        }

        public IActionResult Panel()
        {
            if (HttpContext.Session.GetString("admin_giris") != "ok")
                return RedirectToAction("Login");

            return View();
        }

        [HttpGet("/admin/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_giris");
            return RedirectToAction("Login");
        }
    }
}
