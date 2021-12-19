namespace Ordering.Infrastructure.Mappers;

using Ordering.Domain.AggregateModels.Order;
using Ordering.Infrastructure.Models;
using System;
using System.Collections.Generic;

internal static class OrderMapper
{
    public static Order Map(OrderEntity entity)
    {
        var order = new Order(
            entity.Id,
            Map(entity.Buyer),
            Map(entity.Card),
            MapAddress(entity));

        order.SetCreated(entity.Created);
        order.SetBuyerId(entity.BuyerId);
        order.SetCardId(entity.CardId);

        foreach (var item in entity.OrderItems)
            order.AddOrderItem(Map(item));

        if (entity.OrderStatusId >= OrderStatus.AwaitingValidation.Id)
            order.SetAwaitingValidationStatus();

        if (entity.OrderStatusId >= OrderStatus.StockConfirmed.Id)
            order.SetStockConfirmedStatus();

        if (entity.OrderStatusId == OrderStatus.Paid.Id)
            order.SetPaidStatus();

        if (entity.OrderStatusId == OrderStatus.Cancelled.Id)
            order.SetCancelledStatus();

        order.ClearDomainEvents();

        return order;
    }

    static User Map(BuyerEntity entity)
    {
        return new User(entity.Id, entity.Name);
    }

    static Card Map(CardEntity entity)
    {
        return new Card(
            entity.Type,
            entity.Number,
            entity.SecurityNumber,
            entity.HolderName,
            entity.Expiration);
    }

    static Address MapAddress(OrderEntity entity)
    {
        return new Address(
            entity.Street,
            entity.City,
            entity.Street,
            entity.Country,
            entity.ZipCode);
    }

    static OrderItem Map(OrderItemEntity entity)
    {
        return new OrderItem(
            entity.Id,
            new(entity.ProductName),
            new(entity.UnitPrice),
            new(entity.Units));
    }

    public static OrderEntity Map(Order order)
    {
        var entity = new OrderEntity
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            CardId = order.CardId,
            OrderStatusId = order.Status.Id,
            Description = order.Description,
            Created = order.Created,
            Street = order.Address.Street,
            City = order.Address.City,
            State = order.Address.State,
            Country = order.Address.Country,
            ZipCode = order.Address.ZipCode,
            OrderItems = Map(order.OrderItems, order.Id)
        };

        return entity;
    }

    static List<OrderItemEntity> Map(IEnumerable<OrderItem> items, Guid orderId)
    {
        var list = new List<OrderItemEntity>();
        foreach (var item in items)
            list.Add(Map(item, orderId));

        return list;
    }

    static OrderItemEntity Map(OrderItem item, Guid orderId)
    {
        return new OrderItemEntity
        {
            Id = item.Id,
            OrderId = orderId,
            ProductName = item.ProductName,
            UnitPrice = item.UnitPrice,
            Units = item.Units
        };
    }
}
