namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToPaid;

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

public class PublishIntegrationEventWhenOrderIsPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    readonly IOrderRepository orderRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<PublishIntegrationEventWhenOrderIsPaidDomainEventHandler> logger;

    public PublishIntegrationEventWhenOrderIsPaidDomainEventHandler(
        IOrderRepository orderRepository,
        IIntegrationEventService integrationEventService,
        ILogger<PublishIntegrationEventWhenOrderIsPaidDomainEventHandler> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(notification.OrderId);

        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(order.OrderItems.Select(i => Map(i)));
        logger.LogInformation("Order {OrderId} is paid", order.Id);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }

    OrderStatusChangedToPaidIntegrationEvent.OrderItem Map(OrderItem item)
    {
        return new(item.Id, item.Units);
    }
}
