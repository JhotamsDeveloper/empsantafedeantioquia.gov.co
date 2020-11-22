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
    public class ModelViewNacionLicitante
    {
        [DisplayName("Cod. Id")]
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string NameMaster { get; set; }

        [DisplayName("Url")]
        public string UrlMaster { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Imagen")]
        public string CoverPage { get; set; }

        [DisplayName("F. de inicio")]
        public string NacionLicitantegStartDate { get; set; }

        [DisplayName("F. de finalización")]
        public string NacionLicitanteEndDate { get; set; }

        [DisplayName("Archivo Legales")]
        public IEnumerable<FileDocument> FilesDocuments { get; set; }

        public IEnumerable<Document> DelatedDocuments { get; set; }

        [DisplayName("Fecha de creación")]
        public string DateCreate { get; set; }

        [DisplayName("F. de actualización")]
        public string DateUpdate { get; set; }
    }

}
