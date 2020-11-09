using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace model
{
    public class PQRSD
    {
        public Guid PQRSDID { get; set; }
        public string NamePerson { get; set; }
        public string Email { get; set; }

        public string PQRSDName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string NameSotypeOfRequest { get; set; }
        public DateTime DateCreate { get; set; }
        public string File { get; set; }

        public string Reply { get; set; }
        public Boolean IsAnswered { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
