namespace Ordering.Application.Commands;

using System;
using System.Collections.Generic;

public record CreateOrderDraft(Guid BuyerId, IEnumerable<CreateOrderDraft.OrderItem> OrderItems) : Command<CreateOrderDraft.Draft>
{
    public record Draft(
        IEnumerable<OrderItem> OrderItems,
        decimal Total);

    public record OrderItem(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Units);
}
