namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToAwaitingValidation;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Application.Exceptions;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenOrderIsAwaitingValidationDomainEventHandler : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    readonly IOrderRepository orderRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<PublishIntegrationEventWhenOrderIsAwaitingValidationDomainEventHandler> logger;

    public PublishIntegrationEventWhenOrderIsAwaitingValidationDomainEventHandler(
        IOrderRepository orderRepository,
        IIntegrationEventService integrationEventService,
        ILogger<PublishIntegrationEventWhenOrderIsAwaitingValidationDomainEventHandler> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(notification.OrderId);

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(
            order.Id,
            order.OrderItems.Select(i => Map(i)));

        logger.LogInformation("Order {OrderId} is awaiting validation", order.Id);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }

    static OrderStatusChangedToAwaitingValidationIntegrationEvent.OrderItem Map(OrderItem item)
    {
        return new(item.Id, item.Units);
    }
}
