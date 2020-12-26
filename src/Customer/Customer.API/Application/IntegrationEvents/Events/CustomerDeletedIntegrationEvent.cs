using EventBus.EventBus.Events;

namespace Services.Customer.API.Application.IntegrationEvents.Events
{
    public class CustomerDeletedIntegrationEvent : IntegrationEvent
    {
        public CustomerDeletedIntegrationEvent(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
