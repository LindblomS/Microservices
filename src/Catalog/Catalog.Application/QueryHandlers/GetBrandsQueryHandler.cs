namespace Catalog.Application.QueryHandlers;

using Catalog.Application.Repositories;
using Catalog.Contracts.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IEnumerable<string>>
{
    readonly IQueryRepository repository;

    public GetBrandsQueryHandler(IQueryRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<IEnumerable<string>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.GetBrands());
    }
}
