using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    public class Ayar
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("ayar_adi")]
        public required string AyarAdi { get; set; }

        [Column("icerik")]
        public string? Icerik { get; set; }
    }
}