using Microsoft.AspNetCore.Http;
using modelDTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace modelDTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Occupation { get; set; }
    }

    public class EmployeeCreateDto
    {
        public int EmployeeId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage ="El Nombre es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La Fotografía es requerido")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        [DisplayName("Fotografía")]
        public IFormFile CoverPage { get; set; }

        [Required(ErrorMessage = "El Cargo es requerido")]
        [DisplayName("Cargo")]
        public string Occupation { get; set; }
    }
}
