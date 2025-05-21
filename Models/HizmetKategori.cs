using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    public class HizmetKategori
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("kategori_adi_tr")]
        public string? KategoriAdiTr { get; set; }

        [Column("kategori_adi_en")]
        public string? KategoriAdiEn { get; set; }

        [Column("resim_url")]
        public string? ResimUrl { get; set; }
 
        [Column("baglanti_uzantisi")] 
        public string? BaglantiUzantisi { get; set; }

        public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}