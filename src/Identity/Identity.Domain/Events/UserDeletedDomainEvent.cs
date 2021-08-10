namespace Services.Identity.Domain.Events
{
    using MediatR;
    using Services.Identity.Domain.AggregateModels.User;
    using System;

    public class UserDeletedDomainEvent : INotification
    {
        public UserDeletedDomainEvent(User user)
        {
            Id = user?.Id ?? throw new ArgumentNullException(nameof(user));
        }

        public Guid Id { get; }
    }
}
