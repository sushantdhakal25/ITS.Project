using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Domain.Entities
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int InmateId { get; set; }
        public string InmateName { get; set; }
        public int SourceFacilityId { get; set; }
        public string SourceFacilityName { get; set; }
        public int DestinationFacilityId { get; set; }
        public string DestinationFacilityName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
