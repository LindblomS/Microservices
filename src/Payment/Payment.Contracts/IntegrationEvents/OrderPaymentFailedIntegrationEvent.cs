namespace Payment.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public OrderPaymentFailedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}
