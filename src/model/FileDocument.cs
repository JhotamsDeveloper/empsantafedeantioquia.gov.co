using System;
using System.Collections.Generic;
using System.Text;

namespace model
{
    public class FileDocument
    {
        public int ID { get; set; }
        public string NameFile { get; set; }
        public string RouteFile { get; set; }

        public int? DocumentoId { get; set; }
        public Document Document { get; set; }

        public int? MasterId { get; set; }
        public Master Masters { get; set; }
    }
}
