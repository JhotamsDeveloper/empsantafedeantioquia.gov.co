using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace modelDTOs.CustomValidations
{
    public class ValidateExtensionImg : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string[] _validExtensions = { "JPG", "JPEG", "GIF", "PNG" };

            var file = (IFormFile)value;
            var ext = Path.GetExtension(file.FileName).ToUpper().Replace(".", "");
            return _validExtensions.Contains(ext) && file.ContentType.Contains("image");

        }
    }
}
