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
    public class CategoryDto
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

    public class CategoryCreateDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string NameCategory { get; set; }
        
        [DisplayName("Url")]
        public string UrlCategory { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }
        
        [DisplayName("Portada")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Estado")]
        public bool Statud { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }

    public class CategoryEditDto
    {
        public int Id { get; set; }
        public string NameCategory { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Estado")]
        public bool Statud { get; set; }

        public DateTime DateUpdate { get; set; }
    }
}
