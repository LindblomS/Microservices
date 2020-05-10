using MediatR;
using System;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class CreateServiceCommand : IRequest<bool>
    {
        [DataMember]
        public int FacilityId { get; private set; }

        [DataMember]
        public DateTime StartDate { get; private set; }

        [DataMember]
        public DateTime? StopDate { get; private set; }

        public CreateServiceCommand(int facilityId, DateTime startDate, DateTime stopDate)
        {
            FacilityId = facilityId;
            StartDate = startDate;
            StopDate = stopDate;
        }
    }
}
