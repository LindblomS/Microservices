using CFS.Domain.Aggregates.SharedValueObjects;
using CFS.Domain.SeedWork;
using System;
using System.Collections.Generic;
using CFS.Domain.Aggregates.FacilityAggregate;
using System.Linq;
using CFS.Domain.Events;

namespace CFS.Domain.Aggregates.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private int _phoneNumber;
        private string _email;
        private Address _address;
        private List<Facility> _facilities;

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public int PhoneNumber => _phoneNumber;
        public string Email => _email;
        public Address Address => _address;
        private IReadOnlyList<Facility> Facilities => _facilities;

        public Customer(int id, string firstName, string lastName, int phoneNumber, string email, Address address)
        {
            Id = id;

            _firstName = !string.IsNullOrWhiteSpace(firstName)
                ? firstName
                : throw new ArgumentNullException(nameof(firstName));

            _lastName = !string.IsNullOrWhiteSpace(lastName)
                ? lastName
                : throw new ArgumentNullException(nameof(lastName));

            _phoneNumber = phoneNumber;
            _email = email;
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _facilities = new List<Facility>();
        }

        public void SetAddress(Address address)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void AddFacility(Facility facility)
        {
            var alreadyExists = _facilities.SingleOrDefault(f => f.Id == facility.Id);

            if (alreadyExists == null)
            {
                _facilities.Add(facility);
                var facilityAdded = new CustomerFacilityAddedDomainEvent(Id, facility.Id);
                AddDomainEvent(facilityAdded);
            }
        }

        public void RemoveFacility(Facility facility)
        {
            _facilities.Remove(facility);
            var facilityAddedBefore = DomainEvents.OfType<CustomerFacilityAddedDomainEvent>().SingleOrDefault(f => f.FacilityId == facility.Id);
            if (facilityAddedBefore != null)
            {
                RemoveDomainEvent(facilityAddedBefore);
            }
            else
            {
                var facilityRemoved = new CustomerFacilityAddedDomainEvent(Id, facility.Id);
                AddDomainEvent(facilityRemoved);
            }

        }
    }
}
