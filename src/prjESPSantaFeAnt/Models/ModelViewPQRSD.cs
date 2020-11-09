using modelDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewPQRSD : ReviewCreateDto
    {
        [DisplayName("Código")]
        public Guid PQRSDID { get; set; }

        [DisplayName("Nombre")]
        public string NamePerson { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Asunto")]
        public string PQRSDName { get; set; }

        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("PQRSD")]
        public string NameSotypeOfRequest { get; set; }

        [DisplayName("Creación")]
        public string DateCreate { get; set; }

        [DisplayName("Respuesta")]
        public string ReviewPQRSD { get; set; }

        [DisplayName("Estado")]
        public Boolean IsAnsweredPQRSD { get; set; }
    }
}
