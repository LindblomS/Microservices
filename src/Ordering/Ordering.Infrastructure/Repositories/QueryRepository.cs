namespace Ordering.Infrastructure.Repositories;

using Ordering.Application.Repositories;
using Ordering.Contracts.Requests;
using Ordering.Domain.AggregateModels.Order;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;

public class QueryRepository : IQueryRepository
{
    readonly OrderingContext context;

    public QueryRepository(OrderingContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetOrder.Order> GetOrderAsync(Guid id)
    {
        var order = await context.Orders.FindAsync(id.ToString());
        if (order is null)
            return null;

        return new(order.Id, GetStatus(order.OrderStatusId), order.Description, order.Created);
    }

    public IEnumerable<GetOrders.Order> GetOrders()
    {
        var list = new List<GetOrders.Order>();

        foreach (var order in context.Orders)
            list.Add(new(order.Id, GetStatus(order.OrderStatusId)));

        return list;
    }

    string GetStatus(int id)
    {
        return Enumeration.FromValue<OrderStatus>(id).Name;
    }
}
