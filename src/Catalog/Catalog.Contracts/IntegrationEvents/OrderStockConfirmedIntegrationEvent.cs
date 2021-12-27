namespace Catalog.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public OrderStockConfirmedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}
