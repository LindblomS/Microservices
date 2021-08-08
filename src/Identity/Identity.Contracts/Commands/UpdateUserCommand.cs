namespace Services.Identity.Contracts.Commands
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Results;
    using System;
    using System.Collections.Generic;

    public class UpdateUserCommand : IRequest<CommandResult>
    {
        public UpdateUserCommand(Guid id, IEnumerable<ClaimReadModel> claims, IEnumerable<string> roles)
        {
            Id = id;
            Claims = claims;
            Roles = roles;
        }

        public Guid Id { get; private set; }
        public IEnumerable<ClaimReadModel> Claims { get; private set; }
        public IEnumerable<string> Roles { get; private set; }

    }
}
