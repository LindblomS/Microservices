namespace Services.Identity.Contracts.Commands
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Models.Results;
    using System.Collections.Generic;

    public class CreateUserCommand : IRequest<CommandResult>
    {
        public CreateUserCommand(string username, string passwordHash, IEnumerable<Claim> claims, IEnumerable<string> roles)
        {
            Username = username;
            PasswordHash = passwordHash;
            Claims = claims;
            Roles = roles;
        }

        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }
        public IEnumerable<string> Roles { get; private set; }
    }
}
