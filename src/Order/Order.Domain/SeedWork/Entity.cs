namespace Ordering.Domain.SeedWork;

using MediatR;
using System;
using System.Collections.Generic;

public abstract class Entity
{
    protected Guid id;
    List<INotification> domainEvents;

    public Entity()
    {
        domainEvents = new();
    }

    public Guid Id { get => id; }

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

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Entity))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity)obj;

            return item.Id == id;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Equals(left, null))
            return Equals(right, null) ? true : false;
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
