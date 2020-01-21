using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanctionScanner.Models
{
    public class SanctionModel
    {
        public string LegalName { get; set; }
        public string EntityType { get; set; }
        public string NameType { get; set; }
        public string DateofBirth { get; set; }
        public string PlaceofBirth { get; set; }
        public string Citizenship { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public string ListingInformation { get; set; }
        public string Committees { get; set; }
        public string ControlDate { get; set; }
        public string InsertDate { get; set; }
        public string ModifiedDate { get; set; }
        public byte IsActive { get; set; }
        public string CountryRelated { get; set; }
        public int MatchNumber { get; set; }
        public Guid SactionUID { get; set; }
    }
}
