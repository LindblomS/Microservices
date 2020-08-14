using MediatR;
using System;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class CreateServiceCommand : IRequest<bool>
    {
        [DataMember]
        public int FacilityId { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? StopDate { get; set; }

        public CreateServiceCommand()
        {

        }

        public CreateServiceCommand(int facilityId, DateTime startDate, DateTime stopDate)
        {
            FacilityId = facilityId;
            StartDate = startDate;
            StopDate = stopDate;
        }
    }
}
