namespace Catalog.Application.QueryHandlers;

using Catalog.Application.Repositories;
using Catalog.Contracts.Queries;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GetItemQueryHandler : IRequestHandler<GetItemQuery, Item>
{
    readonly IQueryRepository repository;

    public GetItemQueryHandler(IQueryRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Item> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetItemAsync(request.id);
    }
}
