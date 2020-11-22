using Microsoft.AspNetCore.Http;
using model;
using modelDTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace modelDTOs
{
    public class DocumentDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }
        public string NameProyect { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateUpdate { get; set; }

        public ICollection<FileDocument> FileDocument { get; set; }
    }

    public class DocumentCreateDTO
    {
        [DisplayName("Id")]
        public int ID { get; set; }

        [DisplayName("Nombre del documento")]
        public string Name { get; set; }

        [DisplayName("Url")]
        public string UrlName { get; set; }

        [DisplayName("Nombre de la convocatoria")]
        public string NameProyect { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(2000, ErrorMessage ="Cantidad máximo de tamaño es de 2000 caracteres")]
        [MinLength(150, ErrorMessage = "Caracteres minimos de 150")]
        public string Description { get; set; }

        [DisplayName("Fecha de creación")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Nombre de la convocatoria")]
        [Required]
        public int? MasterId { get; set; }
        public Master Masters { get; set; }

        [DisplayName("Subir documentos en formato .pdf")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public ICollection<IFormFile> RouteFile { get; set; }
        public ICollection<FileDocument> FileDocument { get; set; }
    }

}
