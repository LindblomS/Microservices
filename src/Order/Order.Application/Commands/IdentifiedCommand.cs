namespace Ordering.Application.Commands;

using MediatR;
using System;

public abstract class IdentifiedCommand
{
    public Guid Id { get; set; }
}

public class IdentifiedCommand<TCommand, TResponse> : IdentifiedCommand, IRequest<TResponse>
    where TCommand : IRequest<TResponse>
{
    public IdentifiedCommand(TCommand command, Guid id)
    {
        Command = command;
        Id = id;
    }

    public TCommand Command { get; private set; }
}
