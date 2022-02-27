namespace Ordering.Application.Repositories;

using Ordering.Contracts.Requests;
using System;

public interface IQueryRepository
{
    IEnumerable<GetOrders.Order> GetOrders();
    Task<GetOrder.Order> GetOrderAsync(Guid id);
}
