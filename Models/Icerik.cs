using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    public class Icerik
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("tanim")]
        public required string Tanim { get; set; }

        [Column("tr_description")]
        public string? TrDescription { get; set; }

        [Column("en_description")]
        public string? EnDescription { get; set; }

        public string GetDescription(bool isEnglish)
{
    return isEnglish ? EnDescription ?? "" : TrDescription ?? "";
}
    }
}