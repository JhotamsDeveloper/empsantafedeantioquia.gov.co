using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewBrigade
    {
        public int Id { get; set; }
        
        [DisplayName("Nombre")]
        public string NameMaster { get; set; }

        public string UrlMaster { get; set; }

        [DisplayName("Descripción")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Portada")]
        public string CoverPage { get; set; }

        [DisplayName("Estatus")]
        public Boolean Statud { get; set; }

        [DisplayName("Autor")]
        public string Author { get; set; }

        [DisplayName("Brigada")]
        public string DateBrigade { get; set; }

        [DisplayName("Creación")]
        public string DateCreate { get; set; }

        [DisplayName("Actualización")]
        public DateTime DateUpdate { get; set; }

    }
}
