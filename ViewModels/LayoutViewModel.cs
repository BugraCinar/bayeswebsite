using System.Collections.Generic;
using bayessoft.Models;

namespace bayessoft.Models
{
    public class LayoutViewModel
    {
        public string Dil { get; set; } = "tr"; // "tr" veya "en"

        public bool IsEnglish => Dil == "en";

        public NavbarSection Navbar { get; set; } = new();
        public FooterSection Footer { get; set; } = new();
        public ContactSection Contact { get; set; } = new();
        public SocialSection Socials { get; set; } = new();

        public List<HizmetKategori> HizmetKategorileri { get; set; } = new();

        public Dictionary<string, Icerik> Icerikler { get; set; } = new();

        public HizmetKategori? SeciliKategori { get; set; }
        public List<Hizmet> SeciliKategoriHizmetleri { get; set; } = new();

        public Hizmet? SeciliHizmet { get; set; }
        
        public List<Referans> Referanslar { get; set; } = new();


    }

    public class NavbarSection
    {
        public string Anasayfa { get; set; } = "";
        public string Kurumsal { get; set; } = "";
        public string Hizmetler { get; set; } = "";
        public string Referanslar { get; set; } = "";
        public string Iletisim { get; set; } = "";
    }

    public class FooterSection
    {
        public string KisaYazi { get; set; } = "";
        public string Menu { get; set; } = "";
        public string Iletisim { get; set; } = "";
    }

    public class ContactSection
    {
        public string Adres { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Telefon { get; set; } = "";
    }

    public class SocialSection
    {
        public string Instagram { get; set; } = "";
        public string LinkedIn { get; set; } = "";
    }
}