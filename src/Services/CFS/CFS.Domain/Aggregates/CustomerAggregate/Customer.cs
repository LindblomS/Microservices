using CFS.Domain.SeedWork;
using System;
using CFS.Domain.Events;

namespace CFS.Domain.Aggregates
{
    public class Customer : Entity, IAggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private Address _address;

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string PhoneNumber => _phoneNumber;
        public string Email => _email;
        public Address Address => _address;

        public Customer(int customerId, string firstName, string lastName, string phoneNumber, string email, Address address)
        {
            Id = customerId;

            _firstName = !string.IsNullOrWhiteSpace(firstName)
                ? firstName
                : throw new ArgumentNullException(nameof(firstName));

            _lastName = !string.IsNullOrWhiteSpace(lastName)
                ? lastName
                : throw new ArgumentNullException(nameof(lastName));

            _phoneNumber = phoneNumber;
            _email = email;
            _address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void DeleteCustomer()
        {
            var customerDeletedDomainEvent = new CustomerDeletedDomainEvent(Id);
            AddDomainEvent(customerDeletedDomainEvent);
        }
    }
}
