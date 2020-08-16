using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class UpdateFacilityCommand : IRequest<bool>
    {
        [DataMember]
        public int FacilityId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string FacilityName { get; set; }

        [DataMember]
        public Address Address { get; set; }

        public UpdateFacilityCommand()
        {

        }

        public UpdateFacilityCommand(int facilityId, int customerId, string facilityName, Address address)
        {
            FacilityId = facilityId;
            CustomerId = customerId;
            FacilityName = facilityName;
            Address = address;
        }
    }
}
