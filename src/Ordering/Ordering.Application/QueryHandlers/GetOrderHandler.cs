namespace Ordering.Application.QueryHandlers;

using MediatR;
using Ordering.Application.Repositories;
using Ordering.Contracts.Requests;
using System.Threading;
using System.Threading.Tasks;

public class GetOrderHandler<TRequest, TResponse> : IRequestHandler<GetOrder, GetOrder.Order>
    where TRequest : IRequest
{
    readonly IQueryRepository repository;

    public GetOrderHandler(IQueryRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetOrder.Order> Handle(GetOrder request, CancellationToken cancellationToken)
    {
        return await repository.GetOrderAsync(request.Id);
    }
}
