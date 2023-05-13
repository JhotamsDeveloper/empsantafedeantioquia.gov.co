using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewBlog
    {
        public int Id { get; set; }
        public string NameBlog { get; set; }
        public string UrlMaster { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public Boolean Statud { get; set; }

        public Boolean Blog { get; set; }
        public string Author { get; set; }
        public string DateCreate { get; set; }
        public string DateUpdate { get; set; }
    }
}
