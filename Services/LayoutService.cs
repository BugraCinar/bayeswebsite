using bayessoft.Data;
using bayessoft.Models;

namespace bayessoft.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public LayoutViewModel GetLayoutData(string dil)
        {
            
            var icerikListesi = _context.Icerikler.ToList();
            var icerikler = icerikListesi.ToDictionary(i => i.Tanim, i => i);

            var model = new LayoutViewModel
            {
                Dil = dil,
                Navbar = new NavbarSection
                {
                    Anasayfa = GetIcerik(icerikler, "anasayfa", dil),
                    Kurumsal = GetIcerik(icerikler, "kurumsal", dil),
                    Hizmetler = GetIcerik(icerikler, "hizmetler", dil),
                    Referanslar = GetIcerik(icerikler, "referanslar", dil),
                    Iletisim = GetIcerik(icerikler, "iletisim", dil)
                },
                Footer = new FooterSection
                {
                    KisaYazi = GetIcerik(icerikler, "kisa-yazi", dil),
                    Menu = GetIcerik(icerikler, "menu", dil),
                    Iletisim = GetIcerik(icerikler, "iletisim", dil)
                },
                Contact = new ContactSection
                {
                    Adres = GetAyar("adres"),
                    Mail = GetAyar("mail"),
                    Telefon = GetAyar("tel-no")
                },
                Socials = new SocialSection
                {
                    Instagram = GetAyar("ig-link"),
                    LinkedIn = GetAyar("li-link")
                },
                HizmetKategorileri = _context.HizmetKategorileri.ToList(),

                
                Icerikler = icerikler
            };

                model.Referanslar = _context.Referanslar.ToList();
            return model;
        }

        private string GetIcerik(Dictionary<string, Icerik> kaynak, string tanim, string dil)
        {
            if (kaynak.TryGetValue(tanim, out var item))
            {
                return dil == "en" ? item.EnDescription ?? "" : item.TrDescription ?? "";
            }
            return "";
        }

        private string GetAyar(string adi)
        {
            return _context.Ayarlar.FirstOrDefault(a => a.AyarAdi == adi)?.Icerik ?? "";
        }
    }
}