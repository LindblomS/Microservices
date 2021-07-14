namespace Services.Identity.Domain.AggregateModels.Role
{
    using Services.Identity.Domain.Domain.SeedWork;
    using Services.Identity.Domain.Events;
    using System;

    public class Role : Entity, IAggregateRoot
    {
        public Role(string id, string displayName)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            Id = id;
            DisplayName = displayName;
        }

        public string Id { get; }
        public string DisplayName { get; }

        public void Delete()
        {
            AddDomainEvent(new RoleDeletedDomainEvent(this));
        }
    }
}
