using CFS.Domain.Events;
using CFS.Domain.SeedWork;
using System;

namespace CFS.Domain.Aggregates
{
    public class Facility : Entity, IAggregateRoot
    {
        private int _customerId;
        private string _facilityName;
        private Address _address;

        public int CustomerId => _customerId;
        public string FacilityName => _facilityName;
        public Address Address => _address;

        public Facility(int facilityId, int customerId, string facilityName, Address address)
        {
            Id = facilityId;
            _customerId = customerId;

            _facilityName = !string.IsNullOrEmpty(facilityName)
                ? facilityName
                : throw new ArgumentNullException(nameof(facilityName));

            _address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void Delete()
        {
            var facilityDeletedDomainEvent = new FacilityDeletedDomainEvent(Id);
            AddDomainEvent(facilityDeletedDomainEvent);
        }
    }
}
