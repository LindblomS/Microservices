using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class CreateFacilityCommand : IRequest<bool>
    {
        [DataMember]
        public int CustomerId { get; private set; }

        [DataMember]
        public Address Address { get; private set; }

        public CreateFacilityCommand(int customerId, Address address)
        {
            CustomerId = customerId;
            Address = address;
        }
    }
}
