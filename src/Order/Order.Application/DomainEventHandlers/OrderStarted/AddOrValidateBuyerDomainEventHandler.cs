namespace Ordering.Application.DomainEventHandlers.OrderStarted;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

public class AddOrValidateBuyerDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
{
    readonly IBuyerRepository buyerRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<AddOrValidateBuyerDomainEventHandler> logger;

    public AddOrValidateBuyerDomainEventHandler(
        IBuyerRepository buyerRepository, 
        IIntegrationEventService integrationEventService,
        ILogger<AddOrValidateBuyerDomainEventHandler> logger)
    {
        this.buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var buyer = await AddOrValidateBuyer(notification, cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(notification.Order.Id, notification.Order.Status.Name, buyer.Name);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }

    async Task<Buyer> AddOrValidateBuyer(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var buyer = await buyerRepository.GetAsync(notification.User.Id);
        var buyerExists = buyer is not null;

        if (!buyerExists)
            buyer = new Buyer(notification.User.Id, notification.User.Name);

        buyer.VerifyOrAddCard(
            await GetCard(notification.Card), 
            notification.Order.Id);

        if (!buyerExists)
            await buyerRepository.AddAsync(buyer);

        logger.LogInformation("Buyer {BuyerId} and card were validated or updated for order {OrderId}", buyer.Id, notification.Order.Id);

        _ = await buyerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return buyer;
    }

    async Task<Card> GetCard(Domain.AggregateModels.Order.Card card)
    {
        var existingCard = await buyerRepository.GetCardAsync(
            card.TypeId,
            card.Number,
            card.SecurityNumber,
            card.HolderName,
            card.Expiration);

        if (existingCard is null)
            existingCard = new Card(
                Guid.NewGuid(),
                Enumeration.FromValue<CardType>(card.TypeId),
                card.Number,
                card.SecurityNumber,
                card.HolderName,
                card.Expiration);

        return existingCard;
    }
}
