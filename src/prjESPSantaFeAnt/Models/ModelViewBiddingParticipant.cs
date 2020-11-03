using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewBiddingParticipant
    {

        [DisplayName("Código")]
        public int Id { get; set; }
        public string Ref { get; set; }

        [DisplayName("Persona Natural")]
        public Boolean NaturalPerson { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Cédula y/o Nit")]
        public string IdentificationOrNit { get; set; }

        [DisplayName("Fijo")]
        public string Phone { get; set; }

        [DisplayName("Celular")]
        public string Cellular { get; set; }

        [DisplayName("Dirección")]
        public string Address { get; set; }

        [DisplayName("Ciudad")]
        public string City { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Fecha de creación")]
        public string DateCreate { get; set; }

        [DisplayName("Propuesta")]
        public string Proposals { get; set; }

        [DisplayName("Convocatoria")]
        public string NameNocionLicitante { get; set; }
    }
}
