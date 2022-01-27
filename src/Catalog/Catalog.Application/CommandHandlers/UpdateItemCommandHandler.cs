namespace Catalog.Application.CommandHandlers;

using Catalog.Contracts.Commands;
using Catalog.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, bool>
{
    readonly ICatalogRepository repository;

    public UpdateItemCommandHandler(ICatalogRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
