namespace Catalog.Application.CommandHandlers;

using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, bool>
{
    readonly ICatalogRepository repository;

    public CreateItemCommandHandler(ICatalogRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var type = await repository.GetTypeAsync(request.Type);
        var brand = await repository.GetBrandAsync(request.Brand);
        var item = Create(request, type, brand);
        await repository.AddAsync(item);
        return true;
    }

    static CatalogItem Create(CreateItemCommand item, CatalogType type, CatalogBrand brand)
    {
        return new CatalogItem(
            Guid.NewGuid(),
            item.Name,
            item.Description,
            item.Price,
            type,
            brand,
            item.AvailableStock);
    }
}
