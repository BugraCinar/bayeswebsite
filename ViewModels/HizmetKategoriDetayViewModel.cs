using bayessoft.Models;
using System.Collections.Generic;

namespace bayessoft.ViewModels
{
    public class HizmetKategoriDetayViewModel
    {
        public LayoutViewModel Layout { get; set; } = new();
        public HizmetKategori Kategori { get; set; } = new();
        public List<Hizmet> Hizmetler { get; set; } = new();
    }
}