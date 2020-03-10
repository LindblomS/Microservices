namespace Ordering.API.Application.IntegrationEvents.Events
{
    using EventBus.Events;

    public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public OrderPaymentSucceededIntegrationEvent(int orderId) => OrderId = orderId;
    }
}