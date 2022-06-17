namespace Basket.API.Mappers;

using Basket.Contracts.IntegrationEvents;
using Basket.Contracts.Models;

public static class BasketMapper
{
    public static UserCheckoutAcceptedIntegrationEvent.BasketItem MapEventItem(Domain.AggregateModels.BasketItem item)
    {
        return new(
            item.ProductId,
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
            address.State,
            address.Country,
            address.City,
            address.ZipCode);
    }


}
