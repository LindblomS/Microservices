namespace Catalog.Application.CommandHandlers;

using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, bool>
{
    readonly ICatalogRepository repository;

    public CreateTypeCommandHandler(ICatalogRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<bool> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
    {
        var type = new CatalogType(request.Type);
        await repository.AddAsync(type);
        return true;
    }
}
