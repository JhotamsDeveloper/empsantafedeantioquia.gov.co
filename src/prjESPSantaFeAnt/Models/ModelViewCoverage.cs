using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewCoverage
    {
        [Required(ErrorMessage = "Debe de seleccionar una imagen .jpg")]
        [Display(Name = "Agregar portada")]
        public IFormFile File { get; set; }

        public string Url { get; set; }
    }
}
