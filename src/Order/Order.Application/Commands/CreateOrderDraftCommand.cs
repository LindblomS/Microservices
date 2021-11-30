namespace Order.Application.Commands;

using MediatR;
using System;
using System.Collections.Generic;

class CreateOrderDraftCommand : IRequest<CreateOrderDraftCommand.Draft>
{
    public CreateOrderDraftCommand(Guid buyerId, IEnumerable<OrderItem> orderItems)
    {
        BuyerId = buyerId;
        OrderItems = orderItems;
    }

    public Guid BuyerId { get; private set; }
    public IEnumerable<OrderItem> OrderItems { get; private set; }

    public record Draft(
        IEnumerable<OrderItem> OrderItems,
        decimal Total);

    public record OrderItem(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Units);
}
