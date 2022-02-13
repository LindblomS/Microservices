namespace Catalog.Application.QueryHandlers;

using Catalog.Application.Repositories;
using Catalog.Contracts.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<Item>>
{
    readonly IQueryRepository repository;

    public GetItemsQueryHandler(IQueryRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public Task<IEnumerable<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.GetItems());
    }
}
