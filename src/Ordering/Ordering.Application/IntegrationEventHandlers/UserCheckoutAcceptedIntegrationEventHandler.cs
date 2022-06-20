namespace Ordering.Application.IntegrationEventHandlers;

using Basket.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Services;
using System;
using System.Threading.Tasks;

public class UserCheckoutAcceptedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ICatalogService catalogService;
    readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger;

    public UserCheckoutAcceptedIntegrationEventHandler(
        IMediator mediator, 
        ICatalogService catalogService, 
        ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator;
        this.catalogService = catalogService;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
    {
        await Handle(async () =>
        {
            if (@event.RequestId == default)
            {
                logger.LogWarning("Invalid integration event - RequestId is missing - {@IntegrationEvent}", @event);
                return;
            }

            var items = new List<CreateOrder.OrderItem>();
            var catalogItems = await catalogService.GetAsync(@event.BasketItems.Select(x => x.ProductId));
            foreach (var catalogItem in catalogItems)
                items.Add(Map(catalogItem, @event.BasketItems.Single(x => x.ProductId.ToString() == catalogItem.Id).Units));

            var command = new CreateOrder(
                @event.UserId,
                @event.Username,
                Map(@event.Address),
                Map(@event.Card),
                items);

            var identifiedCommand = new IdentifiedCommand<CreateOrder, bool>(command, @event.RequestId);

            _ = await mediator.Send(identifiedCommand);
        }, @event);

    }

    static CreateOrder.AddressDto Map(UserCheckoutAcceptedIntegrationEvent.AddressDto address)
    {
        return new(
            address.City,
            address.Street,
            address.Country,
            address.ZipCode,
            address.State);
    }

    static CreateOrder.CardDto Map(UserCheckoutAcceptedIntegrationEvent.CardDto card)
    {
        return new(
            card.TypeId,
            card.Number,
            card.HolderName,
            card.SecurityNumber,
            card.Expiration);
    }

    static CreateOrder.OrderItem Map(Catalog.Contracts.Queries.Item item, int units)
    {
        return new(
            Guid.Parse(item.Id),
            item.Name,
            item.Price,
            units);
    }
}
