namespace Services.User.Domain.Events
{
    using MediatR;
    using Services.User.Domain.AggregateModels.Role;
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
