namespace Services.User.API.Application.Models.Commands
{
    using MediatR;
    using Services.User.API.Application.Models.Results;
    using System.Collections.Generic;

    public class UpdateRoleCommand : IRequest<CommandResult>
    {
        public UpdateRoleCommand(string id, string displayName, IEnumerable<Claim> claims)
        {
            Id = id;
            DisplayName = displayName;
            Claims = claims;
        }

        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }
    }
}
