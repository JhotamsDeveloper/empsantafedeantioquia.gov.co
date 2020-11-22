using Microsoft.AspNetCore.Http;
using modelDTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace modelDTOs
{
    public class BlogDto
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
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

    }

    public class BlogCreateDto
    {
        public int Id { get; set; }

        [DisplayName("Título")]
        public string NameBlog { get; set; }
        public string UrlMaster { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }
        public Boolean Statud { get; set; }

        public Boolean Blog { get; set; }

        [DisplayName("Autor")]
        public string Author { get; set; }
        public DateTime DateCreate { get; set; }

    }

    public class BlogEditDto
    {
        public int Id { get; set; }

        [DisplayName("Título")]
        public string NameBlog { get; set; }
        public string UrlMaster { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }
        public Boolean Statud { get; set; }

        public Boolean Blog { get; set; }

        [DisplayName("Autor")]
        public string Author { get; set; }
        public DateTime DateUpdate { get; set; }

    }
}
