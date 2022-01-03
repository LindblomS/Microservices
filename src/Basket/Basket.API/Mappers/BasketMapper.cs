namespace Basket.API.Mappers;

using Basket.API.Models;
using Basket.Contracts.IntegrationEvents;

public static class BasketMapper
{
    public static IEnumerable<BasketItem> Map(IEnumerable<Domain.AggregateModels.BasketItem> items)
    {
        var list = new List<BasketItem>();

        foreach (var item in items)
            list.Add(Map(item));

        return list;
    }

    static BasketItem Map(Domain.AggregateModels.BasketItem item)
    {
        return new BasketItem
        {
            ProductId = item.ProductId.ToString(),
            ProductName = item.ProductName,
            UnitPrice = item.UnitPrice,
            Units = item.Units
        };
    }

    public static Domain.AggregateModels.BasketItem Map(BasketItem item, decimal oldUnitPrice)
    {
        return new(Guid.Parse(item.ProductId), item.ProductName, item.UnitPrice, oldUnitPrice, item.Units);
    }

    public static UserCheckoutAcceptedIntegrationEvent.BasketItem MapEventItem(Domain.AggregateModels.BasketItem item)
    {
        return new(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Units);
    }

    public static UserCheckoutAcceptedIntegrationEvent.CardDto Map(Card card)
    {
        return new(
            card.TypeId,
            card.Number,
            card.SecurityNumber,
            card.HolderName,
            card.Expiration);
    }

    public static UserCheckoutAcceptedIntegrationEvent.AddressDto Map(Address address)
    {
        return new(
            address.Street,
            address.Street,
            address.Country,
            address.City,
            address.ZipCode);
    }


}
