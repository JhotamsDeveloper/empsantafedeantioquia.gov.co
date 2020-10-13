using System;
using System.Collections.Generic;
using System.Text;

namespace services.Commons
{
    public interface IFormatStringUrl
    {
        String FormatString(String texto);
    }

    public class FormatStringUrl:IFormatStringUrl
    {
        public String FormatString(String texto)
        {
            var original = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿ ";
            // Cadena de caracteres ASCII que reemplazarán los originales.
            var ascii = "AAAAAAACEEEEIIIIDNOOOOOOUUUUYBaaaaaaaceeeeiiiionoooooouuuuyy-";
            var output = texto;
            for (int i = 0; i < original.Length; i++)
            {
                // Reemplazamos los caracteres especiales.

                output = output.Replace(original[i], ascii[i]);

            }

            return output.ToLower();
        }
    }
}
