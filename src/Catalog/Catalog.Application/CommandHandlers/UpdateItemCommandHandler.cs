namespace Catalog.Application.CommandHandlers;

using Catalog.Application.Services;
using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateItemCommandHandler : IRequestHandler<InternalUpdateItemCommand, bool>
{
    readonly ICatalogRepository repository;
    readonly DomainEventPublisher domainEventPublisher;

    public UpdateItemCommandHandler(ICatalogRepository repository, DomainEventPublisher domainEventPublisher)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }
    public async Task<bool> Handle(InternalUpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetAsync(request.Id);
        item.ChangePrice(request.Price);

        var updatedItem = new CatalogItem(
            request.Id,
            request.Name,
            request.Description,
            request.Price,
            new CatalogType(request.Type),
            new CatalogBrand(request.Brand),
            request.AvailableStock);

        await repository.UpdateAsync(updatedItem);
        await domainEventPublisher.PublishAsync(item);
        return true;
    }
}
