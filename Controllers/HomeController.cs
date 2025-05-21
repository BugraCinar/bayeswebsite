using Microsoft.AspNetCore.Mvc;
using bayessoft.Services;
using bayessoft.Models;
using Microsoft.EntityFrameworkCore;
using bayessoft.Data;

namespace bayessoft.Controllers
{

    public class HomeController : BaseController
    {

        private readonly AppDbContext _context;
        public HomeController(LayoutService layoutService, AppDbContext context) : base(layoutService) { _context = context; }

        [HttpGet("/")]
        public IActionResult Index()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View(layoutModel);
        }

        [HttpGet("/en")]
        public IActionResult IndexEn()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View("Index", layoutModel);
        }

        [HttpGet("/kurumsal")]
        [HttpGet("/en/kurumsal")]
        public IActionResult Kurumsal()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View(layoutModel);
        }

        [HttpGet("hizmet-kategori")]
        [HttpGet("en/hizmet-kategori")]
        public IActionResult HizmetKategori()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View(layoutModel);
        }

        [HttpGet("/hizmet-kategori/{kategoriUrl}")]
        [HttpGet("/en/hizmet-kategori/{kategoriUrl}")]
        public IActionResult HizmetKategoriDetay(string kategoriUrl)
        {
            var layoutModel = HazirlaLayoutModeli();

            var kategori = _context.HizmetKategorileri
                .FirstOrDefault(k => k.BaglantiUzantisi == kategoriUrl);

            if (kategori == null)
                return NotFound();

            var hizmetler = _context.Hizmetler
                .Where(h => h.KategoriId == kategori.Id)
                .ToList();

            layoutModel.SeciliKategori = kategori;
            layoutModel.SeciliKategoriHizmetleri = hizmetler;

            return View("HizmetKategoriDetay", layoutModel); 
        }

        [HttpGet("/hizmet-kategori/{kategoriUrl}/{hizmetUrl}")]
        [HttpGet("/en/hizmet-kategori/{kategoriUrl}/{hizmetUrl}")]
        public IActionResult HizmetDetay(string kategoriUrl, string hizmetUrl)
        {
            var layoutModel = HazirlaLayoutModeli();

            var hizmet = _context.Hizmetler
                .Include(h => h.Kategori)
                .FirstOrDefault(h =>
                    h.BaglantiUzantisi == hizmetUrl &&
                    h.Kategori!.BaglantiUzantisi == kategoriUrl);

            if (hizmet == null)
                return NotFound();

            layoutModel.SeciliKategori = hizmet.Kategori;
            layoutModel.SeciliHizmet = hizmet;

            return View("HizmetDetay", layoutModel);
        }

        [HttpGet("/referanslar")]
        [HttpGet("/en/referanslar")]
        public IActionResult Referanslar()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View(layoutModel);
        }
        
        [HttpGet("/iletisim")]
        [HttpGet("/en/iletisim")]
        public IActionResult Iletisim()
        {
            var layoutModel = HazirlaLayoutModeli();
            return View(layoutModel);
        }
}
}