using MediatR;
using System;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class UpdateServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; private set; }

        [DataMember]
        public int FacilityId { get; private set; }

        [DataMember]
        public DateTime StartDate { get; private set; }

        [DataMember]
        public DateTime? StopDate { get; private set; }

        public UpdateServiceCommand(int serviceId, int facilityId, DateTime startDate, DateTime stopDate)
        {
            ServiceId = serviceId;
            FacilityId = facilityId;
            StartDate = startDate;
            StopDate = stopDate;
        }
    }
}
