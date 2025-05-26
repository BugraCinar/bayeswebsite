using bayessoft.Data;
using bayessoft.Models;

namespace bayessoft.Services
{
    
    public class LayoutService
    {
        private readonly AppDbContext _context;

        //  enjekte et
        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

       
        public LayoutViewModel GetLayoutData(string dil)
        {
            // İçerikleri tanım bazlı dictionary olarak al
            var icerikListesi = _context.Icerikler.ToList();
            var icerikler = icerikListesi.ToDictionary(i => i.Tanim, i => i);

            // viewModel oluşturuluyor ve gerekli alanlar dolduruluyor burayı aslında kolay yapması ama section olarak ayırmak işi cok kolaylaştırdı
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

                // içerikleri modelde sakla gpt önerdi
                Icerikler = icerikler
            };

            // referanslar da layout verisine dahil ediliyor burayı gpt önermişti
            model.Referanslar = _context.Referanslar.ToList();

            return model;
        }

        //  dil içerik metni 
        private string GetIcerik(Dictionary<string, Icerik> kaynak, string tanim, string dil)
        {
            if (kaynak.TryGetValue(tanim, out var item))
            {
                return dil == "en" ? item.EnDescription ?? "" : item.TrDescription ?? "";
            }
            return "";
        }

        // ayarlar  getir
        private string GetAyar(string adi)
        {
            return _context.Ayarlar.FirstOrDefault(a => a.AyarAdi == adi)?.Icerik ?? "";
        }
    }
}