using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewCategory
    {
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public string NameCategory { get; set; }
        [DisplayName("Descripción")]
        public string Description { get; set; }
        [DisplayName("Portada")]
        public string CoverPage { get; set; }
        [DisplayName("Fecha de creación")]
        public string DateCreate { get; set; }
        [DisplayName("Estado")]
        public bool Statud { get; set; }

    }
}
