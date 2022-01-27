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

    public Task<bool> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
