namespace Services.Customer.Domain
{
    using MediatR;
    using System;

    public class CustomerDeletedDomainEvent : INotification
    {
        public CustomerDeletedDomainEvent(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; private set; }
    }
}
