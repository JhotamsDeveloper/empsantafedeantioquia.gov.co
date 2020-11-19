using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace modelDTOs
{
    public class BrigadeDto
    {
        public int Id { get; set; }
        public string NameMaster { get; set; }
        public string UrlMaster { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public Boolean Statud { get; set; }
        public string Author { get; set; }
        public Boolean Brigade { get; set; }
        public string DateBrigade { get; set; }
        public string DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }

    public class BrigadeCreateDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre de la Brigada")]
        public string NameMaster { get; set; }
        public string UrlMaster { get; set; }

        [DisplayName("Descripción")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Portada")]
        public IFormFile CoverPage { get; set; }
        public Boolean Statud { get; set; }

        [DisplayName("Autor")]
        public string Author { get; set; }
        public Boolean Brigade { get; set; }

        [DisplayName("Fecha de la brigada")]
        public DateTime DateBrigade { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
