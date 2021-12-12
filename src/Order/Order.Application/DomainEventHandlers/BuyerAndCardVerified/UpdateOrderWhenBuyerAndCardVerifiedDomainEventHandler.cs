namespace Ordering.Application.DomainEventHandlers.BuyerAndCardVerified;

using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Exceptions;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateOrderWhenBuyerAndCardVerifiedDomainEventHandler : INotificationHandler<BuyerAndCardVerifiedDomainEvent>
{
    readonly IOrderRepository orderRepository;
    readonly ILogger<UpdateOrderWhenBuyerAndCardVerifiedDomainEventHandler> logger;

    public UpdateOrderWhenBuyerAndCardVerifiedDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<UpdateOrderWhenBuyerAndCardVerifiedDomainEventHandler> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(BuyerAndCardVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(notification.OrderId);

        if (order is null)
            throw new OrderNotFoundException(notification.OrderId);

        order.SetBuyerId(notification.Buyer.Id);
        order.SetCardId(notification.Card.Id);

        logger.LogInformation("Order {OrderId} has been updated with card {CardId}", order.BuyerId, order.CardId);
        await orderRepository.UpdateAsync(order);
    }
}
