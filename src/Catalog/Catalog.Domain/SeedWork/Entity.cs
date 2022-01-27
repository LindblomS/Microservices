namespace Catalog.Domain.SeedWork;

using MediatR;
using System;
using System.Collections.Generic;

public abstract class Entity
{
    List<INotification> domainEvents;

    public Entity()
    {
        domainEvents = new();
    }

    public IReadOnlyCollection<INotification> DomainEvents { get => domainEvents.AsReadOnly(); }

    public void AddDomainEvent(INotification eventItem)
    {
        domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        domainEvents?.Clear();
    }
}
