namespace Catalog.Application.QueryHandlers;

using Catalog.Application.Repositories;
using Catalog.Contracts.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, IEnumerable<string>>
{
    readonly IQueryRepository repository;

    public GetTypesQueryHandler(IQueryRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<IEnumerable<string>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.GetTypes());
    }
}
