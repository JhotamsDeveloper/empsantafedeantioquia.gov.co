using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewPQRSD
    {
        public Guid PQRSDID { get; set; }
        public string NamePerson { get; set; }
        public string Email { get; set; }

        public string PQRSDName { get; set; }
        public string Description { get; set; }
        public string NameSotypeOfRequest { get; set; }
        public string DateCreate { get; set; }
        public string File { get; set; }
    }
}
