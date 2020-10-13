using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace model
{
    public class Category
    {
        public int Id { get; set; }
        public string NameCategory { get; set; }
        public string UrlCategory { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public bool Statud { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
