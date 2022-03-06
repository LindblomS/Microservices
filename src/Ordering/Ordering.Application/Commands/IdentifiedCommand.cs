namespace Ordering.Application.Commands;

using MediatR;
using System;

public abstract record IdentifiedCommand(Guid Id);

public record IdentifiedCommand<TCommand, TResponse> : IdentifiedCommand, IRequest<TResponse>
    where TCommand : Command<TResponse>
{
    public IdentifiedCommand(TCommand command, Guid id) : base(id)
    {
        Command = command;
    }

    public TCommand Command { get; private set; }
}
