using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Domain.Entities
{
    public class Inmate
    {
        public int InmateId { get; set; }
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public int CurrentFacilityId { get; set; }
        public string CurrentFacilityName { get; set; }
    }
}
