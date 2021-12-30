namespace Basket.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;

public class UserCheckoutAcceptedIntegrationEvent : IntegrationEvent
{
    public UserCheckoutAcceptedIntegrationEvent(
        Guid userId,
        string username,
        Guid requestId,
        AddressDto address,
        CardDto card,
        IEnumerable<BasketItem> basketItems)
    {
        UserId = userId;
        Username = username;
        RequestId = requestId;
        Address = address;
        Card = card;
        BasketItems = basketItems;
    }

    public Guid UserId { get; }
    public string Username { get; }
    public Guid RequestId { get; }
    public AddressDto Address { get; }
    public CardDto Card { get; }
    public IEnumerable<BasketItem> BasketItems { get; }

    public record AddressDto(
        string Street,
        string State,
        string Country,
        string City,
        string ZipCode);

    public record CardDto(
        int TypeId,
        string Number,
        string SecurityNumber,
        string HolderName,
        DateTime Expiration);

    public record BasketItem(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Units);
}
