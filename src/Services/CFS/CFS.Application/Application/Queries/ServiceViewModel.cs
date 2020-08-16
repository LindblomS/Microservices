using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFS.Application.Application.Queries
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }
        public int FacilityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? StopDate { get; set; }
    }
}
