using model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace prjESPSantaFeAnt.Models
{
    public class ModelViewDocument
    {
        [DisplayName("Código")]
        public int ID { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Url")]
        public string UrlName { get; set; }

        [DisplayName("Proyecto")]
        public string NameProyect { get; set; }

        [DisplayName("Descripción")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Creación")]
        public string CreateDate { get; set; }

        [DisplayName("Actualización")]
        public string DateUpdate { get; set; }
        public bool ItHasUpdated { get; set; }
        public IEnumerable<FileDocument> FileDocument { get; set; }
    }
}
