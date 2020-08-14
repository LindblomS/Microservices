using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class UpdateCustomerCommand : IRequest<bool>
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Address Address { get; set; }

        public UpdateCustomerCommand()
        {

        }

        public UpdateCustomerCommand(int customerId, string firstName, string lastName, string phoneNumber, string email, Address address)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }
}
