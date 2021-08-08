namespace Services.Identity.Contracts.Commands
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Results;
    using System.Collections.Generic;

    public class CreateRoleCommand : IRequest<CommandResult>
    {
        public CreateRoleCommand(string id, string displayName, IEnumerable<ClaimReadModel> claims)
        {
            Id = id;
            DisplayName = displayName;
            Claims = claims;
        }

        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public IEnumerable<ClaimReadModel> Claims { get; private set; }
    }
}
