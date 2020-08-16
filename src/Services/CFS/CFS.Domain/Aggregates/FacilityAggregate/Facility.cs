using CFS.Domain.Aggregates.CustomerAggregate;
using CFS.Domain.Aggregates.ServiceAggregate;
using CFS.Domain.Aggregates.SharedValueObjects;
using CFS.Domain.Events;
using CFS.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFS.Domain.Aggregates.FacilityAggregate
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
    }
}
