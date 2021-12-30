namespace Basket.API.Services;

using Basket.Contracts.IntegrationEvents;
using Basket.Domain.AggregateModels;
using EventBus.EventBus.Abstractions;
using Grpc.Core;
using System.Threading.Tasks;

public class GrpcBasketService : Protos.Basket.BasketBase
{
    readonly IBasketRepository repository;
    readonly IEventBus eventBus;

    public GrpcBasketService(IBasketRepository repository, IEventBus eventBus)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async override Task<Protos.GetBasketReply> GetBasket(Protos.GetBasketRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out var buyerId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Buyer id was invalid"));

        var basket = await repository.GetBasketAsync(buyerId);

        if (basket is null)
            throw new RpcException(new(StatusCode.NotFound, $"Basket with buyer id {buyerId} does not exists"));

        var reply = new Protos.GetBasketReply();

        foreach (var item in basket.Items)
            reply.BasketItems.Add(Map(item));

        return reply;
    }

    public override async Task<Protos.UpdateBasketReply> UpdateBasket(Protos.UpdateBasketRequest request, ServerCallContext context)
    {
        await repository.CreateUpdateBasketAsync(Map(request));
        return new();
    }

    public override async Task<Protos.CreateCheckoutReply> CreateCheckout(Protos.CreateCheckoutRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out var buyerId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Buyer id was invalid"));

        if (!Guid.TryParse(request.RequestId, out var requestId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Request id was invalid"));

        var basket = await repository.GetBasketAsync(buyerId);

        if (basket is null)
            throw new RpcException(new(StatusCode.NotFound, $"Basket with buyer id {buyerId} does not exists"));

        var integrationEvent = new UserCheckoutAcceptedIntegrationEvent(
            buyerId,
            request.Username,
            requestId,
            Map(request.Address),
            Map(request.Card),
            basket.Items.Select(x => MapEventBasketItem(x)));

        eventBus.Publish(integrationEvent);

        return new();
    }

    public override async Task<Protos.DeleteBasketReply> DeleteBasket(Protos.DeleteBasketRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out var buyerId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Buyer id was invalid"));

        await repository.DeleteBasketAsync(buyerId);

        return new();
    }

    static Basket Map(Protos.UpdateBasketRequest basket)
    {
        if (!Guid.TryParse(basket.BuyerId, out var buyerId))
            throw new RpcException(new(StatusCode.InvalidArgument, "Buyer id was invalid"));

        var domainBasket = new Basket(buyerId);

        foreach (var item in basket.BasketItems)
            domainBasket.AddBasketItem(Map(item));

        return domainBasket;
    }

    static BasketItem Map(Protos.BasketItem item)
    {
        if (!Guid.TryParse(item.ProductId, out var productId))
            throw new RpcException(new(StatusCode.InvalidArgument, $"Product id was invalid for basket item {item}"));

        return new(
            productId,
            item.ProductName,
            Convert.ToDecimal(item.UnitPrice),
            Convert.ToDecimal(item.OldUnitPrice),
            item.Units);
    }

    static Protos.BasketItem Map(BasketItem item)
    {
        return new() 
        { 
            ProductId = item.ProductId.ToString(), 
            ProductName = item.ProductName, 
            UnitPrice = Convert.ToDouble(item.UnitPrice), 
            OldUnitPrice = Convert.ToDouble(item.OldUnitPrice), 
            Units = item.Units 
        };
    }

    static UserCheckoutAcceptedIntegrationEvent.BasketItem MapEventBasketItem(BasketItem item)
    {
        return new(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Units);
    }

    static UserCheckoutAcceptedIntegrationEvent.CardDto Map(Protos.Card card)
    {
        return new(
            card.TypeId,
            card.Number,
            card.SecurityNumber,
            card.HolderName,
            card.Expiration.ToDateTime());
    }

    static UserCheckoutAcceptedIntegrationEvent.AddressDto Map(Protos.Address address)
    {
        return new(
            address.Street,
            address.Street,
            address.Country,
            address.City,
            address.ZipCode);
    }
}
