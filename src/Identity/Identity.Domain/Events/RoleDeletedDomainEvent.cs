namespace Services.Identity.Domain.Events
{
    using Identity.Domain.AggregateModels.Role;
    using MediatR;
    using System;

    public class RoleDeletedDomainEvent : INotification
    {
        public RoleDeletedDomainEvent(Role role)
        {
            Id = role?.Id ?? throw new ArgumentNullException(nameof(role));
        }

        public string Id { get; }
    }
}
