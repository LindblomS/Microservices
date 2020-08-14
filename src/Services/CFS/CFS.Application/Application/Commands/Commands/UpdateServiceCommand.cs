using MediatR;
using System;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class UpdateServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }

        [DataMember]
        public int FacilityId { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? StopDate { get; set; }

        public UpdateServiceCommand()
        {

        }

        public UpdateServiceCommand(int serviceId, int facilityId, DateTime startDate, DateTime stopDate)
        {
            ServiceId = serviceId;
            FacilityId = facilityId;
            StartDate = startDate;
            StopDate = stopDate;
        }
    }
}
