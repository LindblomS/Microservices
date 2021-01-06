namespace Services.Order.API.Application.IntegrationEvents.Events
{
    using EventBus.EventBus.Events;
    using System;

    public class CustomerDeletedIntegrationEvent : IntegrationEvent
    {
        public CustomerDeletedIntegrationEvent(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; private set; }
    }
}
