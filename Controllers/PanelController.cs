using Microsoft.AspNetCore.Mvc;
using bayessoft.Data;
using bayessoft.Models;

namespace bayessoft.Controllers
{
    public class PanelController : Controller
    {
        private readonly AppDbContext _context;

        public PanelController(AppDbContext context)
        {
            _context = context;
        }

        /* 



           ÖNEMLİ >>>

           admin giriş kontrolü yapmam lazımdı ilk başta token ile yapacaktım ama çok ekstraya giriyordu token ile yapmak sonrasında sayfaya girişi
           engellemek için sessionda bilgi tutmayı gördüm ancak tam anlamı ile entegre edemedim, session kontrol kısmında ai yardımı da aldım ama 
           direkt ai komutu ile yapmak yerine elimle yapmayı deneyince tam olmadı session cookies engelleme de çünkü işi bozuyordu o yüzden admin session eksik kaldı  */
        

        
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("admin_giris") == "ok";
        }

        // admin list
        [HttpGet]
        public IActionResult Admin()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var admins = _context.Admins.ToList();
            return PartialView("_AdminList", admins);
        }

        // admin get
        [HttpGet]
        public IActionResult AdminForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            if (id.HasValue && id.Value > 0)
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Id == id.Value);
                if (admin != null)
                    return PartialView("_AdminForm", admin);
            }
            return PartialView("_AdminForm", new Admin { Username = "", PasswordHash = "" });
        }

        // admin post
        [HttpPost]
        public IActionResult AdminForm(Admin admin)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            if (admin.Id == 0)
            {
                // yeni admin 
                admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
                _context.Admins.Add(admin);
            }
            else
            {
                // mevcut admin güncelle
                var existing = _context.Admins.FirstOrDefault(a => a.Id == admin.Id);
                if (existing != null)
                {
                    existing.Username = admin.Username;
                    if (!string.IsNullOrWhiteSpace(admin.PasswordHash))
                        existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
                }
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // ayarlar list
        [HttpGet("/panel/ayarlar")]
        public IActionResult Ayarlar()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var ayarlar = _context.Ayarlar.ToList();
            return PartialView("_AyarList", ayarlar);
        }

        // ayar get
        [HttpGet("/panel/AyarForm")]
        public IActionResult AyarForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            if (id.HasValue && id.Value > 0)
            {
                var ayar = _context.Ayarlar.FirstOrDefault(a => a.Id == id.Value);
                if (ayar != null)
                    return PartialView("_AyarForm", ayar);
            }

            return PartialView("_AyarForm", new Ayar { AyarAdi = "", Icerik = "" });
        }

        // ayar post
        [HttpPost("/panel/AyarForm")]
        public IActionResult AyarForm(Ayar ayar)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            if (ayar.Id == 0)
            {
                _context.Ayarlar.Add(ayar);
            }
            else
            {
                var existing = _context.Ayarlar.FirstOrDefault(a => a.Id == ayar.Id);
                if (existing != null)
                {
                    existing.AyarAdi = ayar.AyarAdi;
                    existing.Icerik = ayar.Icerik;
                }
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // hizmet kategori list
        [HttpGet("/panel/hizmetkategorileri")]
        public IActionResult HizmetKategorileri()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var kategoriler = _context.HizmetKategorileri.ToList();
            return PartialView("_KategoriList", kategoriler);
        }

        // hizmet kategori get
        [HttpGet("/panel/KategoriForm")]
        public IActionResult KategoriForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            if (id.HasValue && id.Value > 0)
            {
                var kategori = _context.HizmetKategorileri.FirstOrDefault(k => k.Id == id.Value);
                if (kategori != null)
                    return PartialView("_KategoriForm", kategori);
            }

            return PartialView("_KategoriForm", new HizmetKategori());
        }

        // hizmet kategori post
        [HttpPost("/panel/KategoriForm")]
        public async Task<IActionResult> KategoriForm(IFormCollection form, IFormFile ResimDosyasi)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            int.TryParse(form["Id"], out int id);

            var kategori = id == 0
                ? new HizmetKategori()
                : _context.HizmetKategorileri.FirstOrDefault(k => k.Id == id);

            if (kategori == null) return BadRequest();

            kategori.KategoriAdiTr = form["KategoriAdiTr"];
            kategori.KategoriAdiEn = form["KategoriAdiEn"];
            kategori.BaglantiUzantisi = form["BaglantiUzantisi"];

            // resim 
            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                if (ResimDosyasi.Length > 5 * 1024 * 1024)
                    return BadRequest("Dosya çok büyük.");

                var fileName = Path.GetFileName(ResimDosyasi.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/kategoriler", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                kategori.ResimUrl = fileName;
            }

            if (id == 0)
                _context.HizmetKategorileri.Add(kategori);

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // kategori sil
        [HttpPost("/panel/KategoriSil")]
        public IActionResult KategoriSil(int id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            var kategori = _context.HizmetKategorileri.FirstOrDefault(k => k.Id == id);
            if (kategori != null)
            {
                if (!string.IsNullOrWhiteSpace(kategori.ResimUrl))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/kategoriler", kategori.ResimUrl);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.HizmetKategorileri.Remove(kategori);
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }

        // hizmet listesi
        [HttpGet("/panel/hizmetler")]
        public IActionResult Hizmetler()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var hizmetler = _context.Hizmetler.ToList();
            return PartialView("_HizmetList", hizmetler);
        }

        // hizmet get
        [HttpGet("/panel/HizmetForm")]
        public IActionResult HizmetForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            var hizmet = id.HasValue && id.Value > 0
                ? _context.Hizmetler.FirstOrDefault(h => h.Id == id.Value)
                : new Hizmet();

            return PartialView("_HizmetForm", hizmet);
        }

        // hizmet post
        [HttpPost("/panel/HizmetForm")]
        public async Task<IActionResult> HizmetForm(IFormCollection form, IFormFile ResimDosyasi)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            int.TryParse(form["Id"], out int id);

            var hizmet = id == 0
                ? new Hizmet()
                : _context.Hizmetler.FirstOrDefault(h => h.Id == id);

            if (hizmet == null) return BadRequest();

            hizmet.UrunAdiTr = form["UrunAdiTr"];
            hizmet.UrunAdiEn = form["UrunAdiEn"];
            hizmet.AciklamaTr = form["AciklamaTr"];
            hizmet.AciklamaEn = form["AciklamaEn"];
            hizmet.BaglantiUzantisi = form["BaglantiUzantisi"];

            if (int.TryParse(form["KategoriId"], out int kategoriId))
                hizmet.KategoriId = kategoriId;
            else
                return BadRequest("Geçersiz kategori.");

            // resim 
            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                if (ResimDosyasi.Length > 5 * 1024 * 1024)
                    return BadRequest("Resim çok büyük.");

                var fileName = Path.GetFileName(ResimDosyasi.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hizmetler", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                hizmet.ResimUrl = fileName;
            }

            if (id == 0)
                _context.Hizmetler.Add(hizmet);

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // hizmet silme
        [HttpPost("/panel/HizmetSil")]
        public IActionResult HizmetSil(int id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == id);
            if (hizmet != null)
            {
                if (!string.IsNullOrWhiteSpace(hizmet.ResimUrl))
                {
                    var path = Path.Combine("wwwroot/images/hizmetler", hizmet.ResimUrl);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.Hizmetler.Remove(hizmet);
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }

        // içerikler list
        [HttpGet("/panel/icerikler")]
        public IActionResult Icerikler()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var liste = _context.Icerikler.ToList();
            return PartialView("_IcerikList", liste);
        }

        // içerik get
        [HttpGet("/panel/IcerikForm")]
        public IActionResult IcerikForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            if (id.HasValue && id.Value > 0)
            {
                var icerik = _context.Icerikler.FirstOrDefault(i => i.Id == id.Value);
                if (icerik == null) return NotFound();

                return PartialView("_IcerikForm", icerik);
            }

            return PartialView("_IcerikForm", new Icerik { Tanim = "", TrDescription = "", EnDescription = "" });
        }

        // içerik post
        [HttpPost("/panel/IcerikForm")]
        public IActionResult IcerikForm(Icerik model)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            if (model.Id == 0)
                _context.Icerikler.Add(model);
            else
            {
                var mevcut = _context.Icerikler.FirstOrDefault(i => i.Id == model.Id);
                if (mevcut != null)
                {
                    mevcut.Tanim = model.Tanim;
                    mevcut.TrDescription = model.TrDescription;
                    mevcut.EnDescription = model.EnDescription;
                }
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // icerik silme
        [HttpPost("/panel/IcerikSil")]
        public IActionResult IcerikSil(int id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var icerik = _context.Icerikler.FirstOrDefault(i => i.Id == id);
            if (icerik != null)
            {
                _context.Icerikler.Remove(icerik);
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }

        // referans list
        [HttpGet("/panel/referanslar")]
        public IActionResult Referanslar()
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var liste = _context.Referanslar.ToList();
            return PartialView("_ReferansList", liste);
        }

        // referans get
        [HttpGet("/panel/ReferansForm")]
        public IActionResult ReferansForm(int? id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var model = id.HasValue && id.Value > 0
                ? _context.Referanslar.FirstOrDefault(r => r.Id == id.Value)
                : new Referans { Isim = "", ResimUrl = "" };

            return PartialView("_ReferansForm", model);
        }

        // referans post
        [HttpPost("/panel/ReferansForm")]
        public async Task<IActionResult> ReferansForm(IFormCollection form)
        {
            if (!IsLoggedIn()) return Redirect("/admin");

            int id = 0;
            int.TryParse(form["Id"], out id);
            string isim = form["Isim"];
            var dosya = form.Files["ResimDosyasi"];

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/referanslar");
            string dosyaAdi = "";

            if (id == 0 && dosya == null)
                return Json(new { success = false, message = "Yeni referans için resim yüklemek zorunludur." });

            if (dosya != null)
            {
                if (dosya.Length > 5 * 1024 * 1024)
                    return Json(new { success = false, message = "Dosya boyutu 5MB'dan büyük olamaz." });

                dosyaAdi = Path.GetFileName(dosya.FileName);
                string yol = Path.Combine(uploadsFolder, dosyaAdi);

                using (var stream = new FileStream(yol, FileMode.Create))
                {
                    await dosya.CopyToAsync(stream);
                }
            }

            if (id == 0)
            {
                var yeni = new Referans { Isim = isim, ResimUrl = dosyaAdi };
                _context.Referanslar.Add(yeni);
            }
            else
            {
                var mevcut = _context.Referanslar.FirstOrDefault(r => r.Id == id);
                if (mevcut != null)
                {
                    mevcut.Isim = isim;
                    if (!string.IsNullOrEmpty(dosyaAdi))
                        mevcut.ResimUrl = dosyaAdi;
                }
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // referans sil
        [HttpPost("/panel/ReferansSil")]
        public IActionResult ReferansSil(int id)
        {
            if (!IsLoggedIn()) return Redirect("/admin");
            var refItem = _context.Referanslar.FirstOrDefault(r => r.Id == id);
            if (refItem != null)
            {
                _context.Referanslar.Remove(refItem);
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }
    }
}
