namespace Catalog.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public OrderStockRejectedIntegrationEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }

    public record OrderItem(Guid ProductId, bool HasStock);
}
