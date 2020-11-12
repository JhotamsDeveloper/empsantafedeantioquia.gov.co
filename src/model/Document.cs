using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace model
{
    public class Document
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }
        public string NameProyect { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateUpdate { get; set; }

        public ICollection<FileDocument> FileDocument { get; set; }

    }
}
