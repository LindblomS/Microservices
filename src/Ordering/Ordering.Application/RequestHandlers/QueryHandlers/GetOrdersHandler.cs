namespace Ordering.Application.RequestHandlers.QueryHandlers;

using MediatR;
using Ordering.Application.Repositories;
using Ordering.Contracts.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<GetOrders.Order>>
{
    readonly IQueryRepository repository;

    public GetOrdersHandler(IQueryRepository repository)
    {
        this.repository = repository;
    }

    public Task<IEnumerable<GetOrders.Order>> Handle(GetOrders request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.GetOrders());
    }
}
