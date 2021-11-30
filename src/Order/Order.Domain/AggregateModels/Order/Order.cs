namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

public class Order : Entity, IAggregateRoot
{
    List<OrderItem> orderItems;
    Address address;
    Guid buyerId;
    Guid paymentMethodId;
    OrderStatus status;
    string description;
    DateTime created;

    Order(Guid id, Address address)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (address is null)
            throw new ArgumentNullException(nameof(address));

        this.id = id;
        status = OrderStatus.Submitted;
        orderItems = new();
        this.address = address;
        created = DateTime.UtcNow;
        description = "";
    }

    public Order(
        Guid id,
        User user,
        Card card,
        Address address) : this(id, address)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (user is null)
            throw new ArgumentNullException(nameof(card));

        AddOrderStartedDomainEvent(user, card);
    }

    public IReadOnlyCollection<OrderItem> OrderItems { get => orderItems.AsReadOnly(); }
    public Address Address { get => address; }
    public Guid BuyerId { get => buyerId; }
    public Guid PaymentMethodId { get => paymentMethodId; }
    public OrderStatus Status { get => status; }
    public string Description { get => description; }
    public DateTime Created { get => created; }

    public void AddOrderItem(OrderItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (status.Id != OrderStatus.Submitted.Id)
            throw new OrderingDomainException($"Cannot add order item when order is {status.Name}");

        var existingItem = orderItems.SingleOrDefault(x => x.Id == item.Id);
        if (existingItem is null)
        {
            orderItems.Add(item);
            return;
        }

        existingItem.AddUnits(1);
    }

    public void SetPaymentId(Guid id)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        paymentMethodId = id;
    }

    public void SetBuyerId(Guid id)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        buyerId = id;
    }

    public void SetAwaitingValidationStatus()
    {
        if (status.Id != OrderStatus.Submitted.Id)
            StatusChangeException(OrderStatus.AwaitingValidation);

        AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(id, orderItems));
        status = OrderStatus.AwaitingValidation;
    }

    public void SetStockConfirmedStatus()
    {
        if (status.Id != OrderStatus.AwaitingValidation.Id)
            StatusChangeException(OrderStatus.StockConfirmed);

        AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(id));
        status = OrderStatus.StockConfirmed;
        description = "All the items were confirmed with available stock.";
    }

    public void SetPaidStatus()
    {
        if (status.Id != OrderStatus.StockConfirmed.Id)
            StatusChangeException(OrderStatus.Paid);

        AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(id, orderItems));
        status = OrderStatus.Paid;
        description = "The payment was performed at a simulated \"Swedish Bank checking bank account ending on XX35071\"";
    }

    public void SetCancelledStatus()
    {
        if (status.Id == OrderStatus.Paid.Id)
            StatusChangeException(OrderStatus.Cancelled);

        AddDomainEvent(new OrderCancelledDomainEvent(id));
        status = OrderStatus.Cancelled;
        description = "The order was cancelled.";
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<Guid> orderStockRejectedItems)
    {
        if (status.Id == OrderStatus.AwaitingValidation.Id)
        {
            status = OrderStatus.Cancelled;

            var itemsStockRejectedProductNames = orderItems
                .Where(c => orderStockRejectedItems.Contains(c.Id))
                .Select(c => c.ProductName);

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    void AddOrderStartedDomainEvent(
        User user,
        Card card)
    {
        AddDomainEvent(new OrderStartedDomainEvent(this, user, card));
    }

    void StatusChangeException(OrderStatus status)
    {
        throw new OrderingDomainException($"Is not possible to change the order status from {this.status.Name} to {status.Name}.");
    }
}
