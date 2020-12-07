using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewEmployees
    {
        public int EmployeeId { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Fotografía")]
        public string CoverPage { get; set; }
        [DisplayName("Cargo")]
        public string Occupation { get; set; }
    }
}
