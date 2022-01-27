namespace Catalog.Application.CommandHandlers;

using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class DeleteCatalogItemCommandHandler : IRequestHandler<DeleteCatalogItemCommand, bool>
{
    readonly ICatalogRepository repository;

    public DeleteCatalogItemCommandHandler(ICatalogRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<bool> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.id);
        return true;
    }
}
