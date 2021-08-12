namespace Services.User.API.Application.Models.Commands
{
    using MediatR;
    using Services.User.API.Application.Models.Results;
    using System;
    using System.Collections.Generic;

    public class UpdateUserCommand : IRequest<CommandResult>
    {
        public UpdateUserCommand(Guid id, IEnumerable<Claim> claims, IEnumerable<string> roles)
        {
            Id = id;
            Claims = claims;
            Roles = roles;
        }

        public Guid Id { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }
        public IEnumerable<string> Roles { get; private set; }

    }
}
