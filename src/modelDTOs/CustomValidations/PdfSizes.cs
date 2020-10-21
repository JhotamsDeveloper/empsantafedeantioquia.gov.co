using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace modelDTOs.CustomValidations
{
    public class PdfSizes : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var file = (IFormFile)value;
            var size = 1048576 * 10;

            if (size < file.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
