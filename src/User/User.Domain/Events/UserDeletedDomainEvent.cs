namespace Services.User.Domain.Events
{
    using MediatR;
    using Services.User.Domain.AggregateModels.User;
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
