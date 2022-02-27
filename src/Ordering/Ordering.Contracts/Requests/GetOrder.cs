namespace Ordering.Contracts.Requests;

using MediatR;
using System;

public record GetOrder(Guid Id) : IRequest<GetOrder.Order>
{
    public record Order(
        string Id,
        string Status,
        string Description,
        DateTime Created);
}
