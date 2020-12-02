using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string UrlProduct { get; set; }
        public string Icono { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime DateCreate { get; set; }
    }
}
