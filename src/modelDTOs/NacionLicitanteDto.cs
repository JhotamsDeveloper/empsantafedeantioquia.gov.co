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
    public class NacionLicitanteDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string NameMaster { get; set; }
        
        [DisplayName("Url")]
        public string UrlMaster { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        public string CoverPage { get; set; }

        [DisplayName("Estado")]
        public Boolean Statud { get; set; }

        public Boolean NacionLicitante { get; set; }

        [DisplayName("Fecha inicio de la convocatoria")]
        public DateTime NacionLicitantegStartDate { get; set; }

        [DisplayName("Fecha fin de la convocatoria")]
        public DateTime NacionLicitanteEndDate { get; set; }

        [DisplayName("Archivos legales")]
        public string NacionLicitantegFile { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }

    public class NacionLicitanteCreateDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string NameMaster { get; set; }
        
        [DisplayName("Url")]
        public string UrlMaster { get; set; }
        
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [Required]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }
        
        [DisplayName("Estado")]
        public Boolean Statud { get; set; }

        [DisplayName("Licitación")]
        public Boolean NacionLicitante { get; set; }

        [DisplayName("Fecha de inicio de la licitación")]
        [Required]
        public DateTime NacionLicitantegStartDate { get; set; }

        [DisplayName("Fecha fin de la licitación")]
        [Required]
        public DateTime NacionLicitanteEndDate { get; set; }

        [DisplayName("Archivos legales")]
        [Required]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public ICollection<IFormFile> NacionLicitantegFile { get; set; }

        [DisplayName("Fecha de Creación")]
        public DateTime DateCreate { get; set; }

    }

    public class NacionLicitanteEditDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string NameMaster { get; set; }

        [DisplayName("Url")]
        public string UrlMaster { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Estado")]
        public Boolean Statud { get; set; }

        public Boolean NacionLicitante { get; set; }

        [Required]
        [DisplayName("Fecha de inicio de la licitación")]
        public DateTime NacionLicitantegStartDate { get; set; }
        
        [Required]
        [DisplayName("Fecha de fin de la licitación")]
        public DateTime NacionLicitanteEndDate { get; set; }

        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        [DisplayName("Archivos legales")]
        public IFormFile NacionLicitantegFile { get; set; }

        public DateTime DateUpdate { get; set; }
    }
}
