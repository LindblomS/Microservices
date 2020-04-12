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
        private Address _address;
        private List<Service> _services;

        public Address Address => _address;
        public IReadOnlyList<Service> Services => _services;

        public Facility(int id, Address address)
        {
            Id = id;
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
