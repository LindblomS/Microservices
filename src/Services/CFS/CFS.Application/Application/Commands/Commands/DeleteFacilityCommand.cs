using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class DeleteFacilityCommand : IRequest<bool>
    {
        [DataMember]
        public int FacilityId { get; set; }

        public DeleteFacilityCommand()
        {

        }

        public DeleteFacilityCommand(int facilityId)
        {
            FacilityId = facilityId;
        }
    }
}
