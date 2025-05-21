using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    public class Referans
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("resim_url")]
        public string? ResimUrl { get; set; }

        [Column("isim")]
        public string? Isim { get; set; }
    }
}