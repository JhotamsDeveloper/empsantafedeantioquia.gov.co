using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace model
{
    public class Master
    {
        public int Id { get; set; }
        public string NameMaster { get; set; }
        public string UrlMaster { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public Boolean Statud { get; set; }

        public Boolean Brigade { get; set; }
        public DateTime DateBrigade { get; set; }

        public Boolean Blog { get; set; }
        public string Author { get; set; }

        public Boolean NacionLicitante { get; set; }
        public DateTime NacionLicitantegStartDate { get; set; }
        public DateTime NacionLicitanteEndDate { get; set; }
        public string NacionLicitantegFile { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public IList<BiddingParticipant> BiddingParticipants { get; set; }

    }
}
