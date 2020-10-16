using System;
using System.Collections.Generic;
using System.Text;

namespace model
{
    public class BiddingParticipant
    {
        public int Id { get; set; }
        public Boolean NaturalPerson { get; set; }
        public string Name { get; set; }
        public string IdentificationOrNit { get; set; }
        public string Phone { get; set; }
        public string Cellular { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }
        public string Proposals { get; set; }

        public int? MasterId { get; set; }
        public Master Master { get; set; }
    }
}
