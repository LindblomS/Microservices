namespace Services.Customer.Domain
{
    using Services.Customer.Domain.SeedWork;
    using System;

    public class Customer : Entity, IAggregateRoot
    {
        private string _name;

        public Customer(Guid id, string name)
        {
            if (id == default(Guid))
                throw new ArgumentException(nameof(id));

            Id = id;
            _name = name;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }

        public void Delete()
        {
            var @event = new CustomerDeletedDomainEvent(Id);
            AddDomainEvent(@event);
        }
    }
}
