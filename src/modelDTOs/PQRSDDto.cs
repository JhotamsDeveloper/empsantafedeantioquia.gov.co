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
    public class PQRSDDto
    {
        public Guid PQRSDID { get; set; }
        public string NamePerson { get; set; }
        public string Email { get; set; }

        public string PQRSDName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Descriptio { get; set; }
        public string NameSotypeOfRequest { get; set; }
        public DateTime DateCreate { get; set; }
        public string File { get; set; }

    }

    public class PQRSDCreateDto
    {
        public Guid PQRSDID { get; set; }
        public string NamePerson { get; set; }
        public string Email { get; set; }

        public string PQRSDName { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string NameSotypeOfRequest { get; set; }
        public DateTime DateCreate { get; set; }

        [DisplayName("Subir el PQRSD en .pdf *")]
        [PdfSizes(ErrorMessage = "El tamaño no debe de ser superior a 10mb")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public IFormFile File { get; set; }

    }

    public class PQRSDEditDto
    {
        public int ID { get; set; }
        public string NamePerson { get; set; }
        public string Email { get; set; }

        public string PQRSDName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string NameSotypeOfRequest { get; set; }
        public DateTime DateCreate { get; set; }

        [DisplayName("Subir el PQRSD en .pdf *")]
        [PdfSizes(ErrorMessage = "El tamaño no debe de ser superior a 10mb")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public IFormFile File { get; set; }


    }
}
