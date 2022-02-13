namespace Catalog.Application.CommandHandlers;

using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, bool>
{
    readonly ICatalogRepository repository;

    public CreateBrandCommandHandler(ICatalogRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new CatalogBrand(request.Brand);
        await repository.AddAsync(brand);
        return true;
    }
}
