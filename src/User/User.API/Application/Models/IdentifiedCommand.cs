namespace Services.User.API.Application.Models
{
    using MediatR;
    using System;

    public class IdentifiedCommand<T, R> : IRequest<R> where T : IRequest<R>
    {
        public IdentifiedCommand(T command, Guid id)
        {
            Command = command;
            Id = id;
        }

        public T Command { get; set; }
        public Guid Id { get; set; }
    }
}
