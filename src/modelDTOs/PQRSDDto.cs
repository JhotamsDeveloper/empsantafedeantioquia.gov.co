using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DisplayName("Nombre *")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        public string NamePerson { get; set; }

        [DisplayName("Email *")]
        [Required(ErrorMessage = "El email es requerido."), MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Asunto *")]
        [Required(ErrorMessage = "El Asunto es requerido."), MaxLength(150)]
        public string PQRSDName { get; set; }

        [AllowHtml]
        [DisplayName("Descripción *")]
        [Required(ErrorMessage = "La descripción es requerido.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Tipo de PQRSD *")]
        [Required(ErrorMessage = "El tipo de PQRSD es requerido.")]
        public string NameSotypeOfRequest { get; set; }
        public DateTime DateCreate { get; set; }

        public IFormFile File { get; set; }

    }

    public class ReviewCreateDto
    {
        public Guid ID { get; set; }

        [DisplayName("Respuesta")]
        public string Reply { get; set; }
        public Boolean IsAnswered { get; set; }
        [DisplayName("Fecha de respuesta")]
        public string AnswerDate { get; set; }
    }
}
