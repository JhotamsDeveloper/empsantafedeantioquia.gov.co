using Microsoft.AspNetCore.Http;
using model;
using modelDTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace modelDTOs
{
    public class BiddingParticipantDTO
    {
        public int Id { get; set; }
        public string Ref { get; set; }
        public Boolean NaturalPerson { get; set; }
        public string Name { get; set; }
        public string IdentificationOrNit { get; set; }
        public string Phone { get; set; }
        public string Cellular { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }
        public string Proposals { get; set; }
    }

    public class BiddingParticipantCreateDTO
    {
        public int Id { get; set; }

        [DisplayName("¿Es persona natural?")]
        public Boolean NaturalPerson { get; set; }
        public string Ref { get; set; }

        [DisplayName("Nombre Completo *")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        public string Name { get; set; }

        [DisplayName("Cedula o Nit *")]
        [Required(ErrorMessage = "Este campo es necesario."), MaxLength(20)]
        public string IdentificationOrNit { get; set; }

        [DisplayName("Fijo *")]
        [Required(ErrorMessage = "Este campo es necesario")]
        public string Phone { get; set; }

        [DisplayName("Celular *")]
        [Required(ErrorMessage = "Este campo es necesario."), MaxLength(10)]
        public string Cellular { get; set; }

        [DisplayName("Dirección física *")]
        [Required(ErrorMessage = "Este campo es necesario."), MaxLength(150)]
        public string Address { get; set; }

        [DisplayName("Ciudad *")]
        [Required(ErrorMessage = "Este campo es necesario."), MaxLength(50)]
        public string City { get; set; }

        [DisplayName("E-mail *")]
        [EmailAddress(ErrorMessage = "Formato inválido*")]
        public string Email { get; set; }

        public DateTime DateCreate { get; set; }

        [DisplayName("Subir la propuesta en .pdf *")]
        [PdfSizes(ErrorMessage = "El tamaño no debe de ser superior a 10mb")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public IFormFile Proposals { get; set; }
        public int? MasterId { get; set; }
        public Master Master { get; set; }
    }
}
