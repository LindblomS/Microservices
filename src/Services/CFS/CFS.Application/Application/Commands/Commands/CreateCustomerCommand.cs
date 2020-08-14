using CFS.Application.Application.Models;
using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class CreateCustomerCommand : IRequest<bool>
    {
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

        public CreateCustomerCommand()
        {
            
        }

        public CreateCustomerCommand(string firstName, string lastName, string phoneNumber, string email, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }
}
