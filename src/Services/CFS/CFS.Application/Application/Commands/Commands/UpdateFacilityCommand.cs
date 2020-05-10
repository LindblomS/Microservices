using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class UpdateFacilityCommand : IRequest<bool>
    {
        [DataMember]
        public int FacilityId { get; private set; }

        [DataMember]
        public int CustomerId { get; private set; }

        [DataMember]
        public Address Address { get; private set; }

        public UpdateFacilityCommand(int facilityId, int customerId, Address address)
        {
            FacilityId = facilityId;
            CustomerId = customerId;
            Address = address;
        }
    }
}
