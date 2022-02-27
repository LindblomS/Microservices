namespace Ordering.Contracts.Requests;

using MediatR;
using System.Collections.Generic;

public record GetOrders() : IRequest<IEnumerable<GetOrders.Order>>
{
    public record Order(
        string Id,
        string Status);
}
