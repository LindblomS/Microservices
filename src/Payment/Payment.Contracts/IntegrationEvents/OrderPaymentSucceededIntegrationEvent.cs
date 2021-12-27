namespace Payment.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public OrderPaymentSucceededIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}
