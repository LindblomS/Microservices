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
        private Address _address;

        public int CustomerId => _customerId;
        public Address Address => _address;

        public Facility(int facilityId, int customerId, Address address)
        {
            Id = facilityId;
            _customerId = customerId;
            _address = address ?? throw new ArgumentNullException(nameof(address));
        }
    }
}
