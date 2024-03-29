﻿namespace Ordering.Application.DomainEventHandlers.OrderStarted;

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
    readonly DomainEventPublisher domainEventPublisher;

    public AddOrValidateBuyerDomainEventHandler(
        IBuyerRepository buyerRepository, 
        IIntegrationEventService integrationEventService,
        ILogger<AddOrValidateBuyerDomainEventHandler> logger,
        DomainEventPublisher domainEventPublisher)
    {
        this.buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var buyer = await AddOrValidateBuyer(notification, cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(notification.Order.Id, notification.Order.Status.Name, buyer.Name);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }

    async Task<Buyer> AddOrValidateBuyer(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var buyer = await buyerRepository.GetAsync(notification.User.Id, notification.Order.Id);
        var buyerExists = buyer is not null;

        if (!buyerExists)
            buyer = new Buyer(notification.User.Id, notification.User.Name);

        buyer.VerifyOrAddCard(
            GetCard(notification.Card), 
            notification.Order.Id);

        if (!buyerExists)
            await buyerRepository.AddAsync(buyer);
        else 
            await buyerRepository.UpdateAsync(buyer);

        logger.LogInformation("Buyer {BuyerId} and card were validated or updated for order {OrderId}", buyer.Id, notification.Order.Id);

        await domainEventPublisher.PublishAsync(buyer);

        return buyer;
    }

    Card GetCard(Domain.AggregateModels.Order.Card card)
    {
        var existingCard = buyerRepository.GetCard(
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
