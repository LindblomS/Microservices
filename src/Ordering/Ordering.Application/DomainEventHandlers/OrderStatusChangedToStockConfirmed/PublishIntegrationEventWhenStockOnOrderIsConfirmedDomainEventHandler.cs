﻿namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToStockConfirmed;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Application.Exceptions;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Domain.Events;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenStockOnOrderIsConfirmedDomainEventHandler : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    readonly IOrderRepository orderRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<PublishIntegrationEventWhenStockOnOrderIsConfirmedDomainEventHandler> logger;

    public PublishIntegrationEventWhenStockOnOrderIsConfirmedDomainEventHandler(
        IOrderRepository orderRepository,
        IIntegrationEventService integrationEventService,
        ILogger<PublishIntegrationEventWhenStockOnOrderIsConfirmedDomainEventHandler> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(notification.OrderId);

        var integrationEvent = new OrderStatusChangedToStockConfirmedIntegrationEvent(order.Id);

        logger.LogInformation("Stock on order {OrderId} has been confirmed", order.Id);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
