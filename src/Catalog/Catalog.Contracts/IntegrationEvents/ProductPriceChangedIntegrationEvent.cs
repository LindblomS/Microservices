namespace Catalog.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public ProductPriceChangedIntegrationEvent(Guid productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }

    public Guid ProductId { get; private set; }
    public decimal NewPrice { get; private set; }
}
