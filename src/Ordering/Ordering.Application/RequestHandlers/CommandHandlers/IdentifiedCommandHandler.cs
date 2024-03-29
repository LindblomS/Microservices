﻿namespace Ordering.Application.RequestHandlers.CommandHandlers;

using MediatR;
using Ordering.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ordering.Application.Commands;

public class IdentifiedCommandHandler<TCommand, TResponse> : IRequestHandler<IdentifiedCommand<TCommand, TResponse>, TResponse>
    where TCommand : Command<TResponse>
{
    readonly IRequestManager requestManager;
    readonly IMediator mediator;

    public IdentifiedCommandHandler(
        IRequestManager requestManager,
        IMediator mediator)
    {
        this.requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task<TResponse> Handle(IdentifiedCommand<TCommand, TResponse> request, CancellationToken cancellationToken)
    {
        var exists = await requestManager.ExistsAsync(request.Id);

        if (exists)
            return CreateResultForDuplicateRequest();

        await requestManager.CreateRequestAsync<TCommand>(request.Id);
        return await mediator.Send(request.Command);
    }

    protected virtual TResponse CreateResultForDuplicateRequest()
    {
        return default;
    }
}
