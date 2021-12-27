namespace Ordering.Application.IntegrationEventHandlers;

using Basket.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using System;
using System.Threading.Tasks;

public class UserCheckoutAcceptedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger;

    public UserCheckoutAcceptedIntegrationEventHandler(IMediator mediator, ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator;
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

            var command = new CreateOrderCommand(
                @event.UserId,
                @event.Username,
                Map(@event.Address),
                Map(@event.Card),
                @event.BasketItems.Select(x => Map(x)));

            var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, bool>(command, @event.RequestId);

            _ = await mediator.Send(identifiedCommand);
        }, @event);

    }

    static CreateOrderCommand.AddressDto Map(UserCheckoutAcceptedIntegrationEvent.AddressDto address)
    {
        return new(
            address.City,
            address.Street,
            address.Country,
            address.ZipCode,
            address.State);
    }

    static CreateOrderCommand.CardDto Map(UserCheckoutAcceptedIntegrationEvent.CardDto card)
    {
        return new(
            card.TypeId,
            card.Number,
            card.HolderName,
            card.SecurityNumber,
            card.Expiration);
    }

    static CreateOrderCommand.OrderItem Map(UserCheckoutAcceptedIntegrationEvent.BasketItem item)
    {
        return new(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Units);
    }
}
