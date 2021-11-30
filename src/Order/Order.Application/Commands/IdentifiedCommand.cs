namespace Ordering.Application.Commands;

using MediatR;
using System;

public class IdentifiedCommand<TCommand, TResponse> : IRequest<TResponse>
    where TCommand : IRequest<TResponse>
{
    public IdentifiedCommand(TCommand command, Guid id)
    {
        Command = command;
        Id = id;
    }

    public TCommand Command { get; private set; }
    public Guid Id { get; private set; }
}
