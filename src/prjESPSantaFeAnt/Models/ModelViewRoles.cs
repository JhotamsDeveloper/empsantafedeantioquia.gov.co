using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewRoles
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
