using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string UrlProduct { get; set; }
        public string Icono { get; set; }
        public string Description { get; set; }
        public string DateCreate { get; set; }
    }
}
