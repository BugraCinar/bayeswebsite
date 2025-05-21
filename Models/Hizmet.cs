using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    public class Hizmet
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("urun_adi_tr")]
        public string? UrunAdiTr { get; set; }

        [Column("urun_adi_en")]
        public string? UrunAdiEn { get; set; }

        [Column("aciklama_tr")]
        public string? AciklamaTr { get; set; }

        [Column("aciklama_en")]
        public string? AciklamaEn { get; set; }

        [Column("resim_url")]
        public string? ResimUrl { get; set; }

        [Column("baglanti_uzantisi")]
        public string? BaglantiUzantisi { get; set; }

        [Column("kategori_id")]
        public int KategoriId { get; set; }

        public HizmetKategori? Kategori { get; set; }
    }
}