using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class CreateFacilityCommand : IRequest<bool>
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string FacilityName { get; set; }

        [DataMember]
        public Address Address { get; set; }

        public CreateFacilityCommand()
        {

        }

        public CreateFacilityCommand(int customerId, string facilityName, Address address)
        {
            CustomerId = customerId;
            FacilityName = facilityName;
            Address = address;
        }
    }
}
