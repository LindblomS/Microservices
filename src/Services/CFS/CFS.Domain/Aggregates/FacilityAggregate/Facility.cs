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
        private List<Service> _services;

        public int CustomerId => _customerId;
        public Address Address => _address;
        public IReadOnlyList<Service> Services => _services;

        public Facility(int facilityId, int customerId, Address address)
        {
            Id = facilityId;
            _customerId = customerId;
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _services = new List<Service>();
        }

        public void SetAddress(Address address)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void AddService(Service service)
        {
            var alreadyExists = _services.SingleOrDefault(s => s.Id == service.Id);

            if (alreadyExists == null)
            {
                _services.Add(service);
                var serviceAdded = new FacilityServiceAddedDomainEvent(Id, service.Id);
                AddDomainEvent(serviceAdded);
            }
        }

        public void RemoveService(Service service)
        {
            _services.Remove(service);

            var serviceAddedBefore = DomainEvents.OfType<FacilityServiceAddedDomainEvent>().SingleOrDefault(e => e.ServiceId == service.Id);
            if (serviceAddedBefore != null)
            {
                RemoveDomainEvent(serviceAddedBefore);
            }
            else
            {
                var serviceRemoved = new FacilityServiceRemovedDomainEvent(Id, service.Id);
                AddDomainEvent(serviceRemoved);
            }
        }
    }
}
