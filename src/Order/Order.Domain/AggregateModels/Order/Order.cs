namespace Services.Order.Domain.AggregateModels.Order;

using Services.Order.Domain.Events;
using Services.Order.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Order : Entity, IAggregateRoot
{
    List<OrderItem> orderItems;
    Address adress;
    Guid buyerId;
    Guid paymentMethodId;
    OrderStatus status;
    string description;
    DateTime created;

    public Order(Guid id)
    {
        this.id = id;
        status = OrderStatus.Submitted;
        orderItems = new List<OrderItem>();
        created = DateTime.UtcNow;
    }

    public Order(
        Guid id,
        Guid userId,
        string username,
        Card card) : this(id)
    {
        AddOrderStartedDomainEvent(userId, username, card);
    }

    void AddOrderStartedDomainEvent(
        Guid userId,
        string username,
        Card card)
    {
        AddDomainEvent(new OrderStartedDomainEvent(this, userId, username, card));
    }
}
