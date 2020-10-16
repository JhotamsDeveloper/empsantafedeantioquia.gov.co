using Microsoft.AspNetCore.Http;
using modelDTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.Text;

namespace modelDTOs
{
    public class BiddingParticipantDTOs
    {
        public int Id { get; set; }
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

    public class BiddingParticipantCreateDTOs
    {
        public int Id { get; set; }
        public Boolean NaturalPerson { get; set; }
        public string Name { get; set; }
        public string IdentificationOrNit { get; set; }
        public string Phone { get; set; }
        public string Cellular { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }

        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        [ValidateExtensionPDF(new string[] { ".pdf" })]
        public IFormFile Proposals { get; set; }
    }
}
