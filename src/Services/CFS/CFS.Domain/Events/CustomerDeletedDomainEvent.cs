using MediatR;

namespace CFS.Domain.Events
{
    public class CustomerDeletedDomainEvent : INotification
    {
        public int CustomerId { get; private set; }

        public CustomerDeletedDomainEvent(int customerId)
        {
            CustomerId = CustomerId;
        }
    }
}
