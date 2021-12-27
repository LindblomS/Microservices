namespace Ordering.Application.DomainEventHandlers.OrderCancelled;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Application.Exceptions;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenOrderIsCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    readonly IOrderRepository orderRepository;
    readonly IBuyerRepository buyerRepository;
    readonly IIntegrationEventService integrationEventService;
    readonly ILogger<PublishIntegrationEventWhenOrderIsCancelledDomainEventHandler> logger;

    public PublishIntegrationEventWhenOrderIsCancelledDomainEventHandler(
        IOrderRepository orderRepository,
        IBuyerRepository buyerRepository,
        IIntegrationEventService integrationEventService,
        ILogger<PublishIntegrationEventWhenOrderIsCancelledDomainEventHandler> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        this.integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(notification.OrderId);

        if (order is null)
            throw new OrderNotFoundException(notification.OrderId);

        var buyer = await buyerRepository.GetAsync(order.BuyerId, order.Id);

        if (buyer is null)
            throw new BuyerNotFoundException(order.BuyerId);

        var integrationEvent = new OrderStatusChangedToCancelledIntegrationEvent(
            order.Id,
            order.Status.Name,
            buyer.Name);

        logger.LogInformation("Order {OrderId} is cancelled", order.Id);
        await integrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
