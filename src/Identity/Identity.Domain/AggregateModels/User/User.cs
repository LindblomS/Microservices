namespace Services.Identity.Domain.AggregateModels.User
{
    using Services.Identity.Domain.AggregateModels.Role;
    using Services.Identity.Domain.Events;
    using Services.Identity.Domain.Exceptions;
    using Services.Identity.Domain.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class User : Entity, IAggregateRoot
    {
        private readonly Guid _id;
        private readonly string _username;
        private List<Claim> _claims;
        private List<Role> _roles;

        public User(string username)
        {
            _id = Guid.NewGuid();

            ValidateUsername(username);
            _username = username;

            _claims = new List<Claim>();
            _roles = new List<Role>();
        }

        public User(Guid id, string username, IEnumerable<Claim> claims, IEnumerable<Role> roles)
        {
            if (id == default(Guid))
                throw new ArgumentNullException(nameof(id));

            _id = id;

            ValidateUsername(username);
            _username = username;

            _claims = claims?.ToList() ?? throw new ArgumentNullException(nameof(claims));
            _roles = roles?.ToList() ?? throw new ArgumentNullException(nameof(roles));
        }

        public Guid Id => _id;
        public string Username => _username;
        public IReadOnlyList<Claim> Claims => _claims;
        public IReadOnlyList<Role> Roles => _roles;

        public void AddRole(Role role)
        {
            if (_roles.Any(x => x.Id == role.Id))
                throw new IdentityDomainException("Cannot add role to user. User already has role");

            _roles.Add(role);
        }

        public void RemoveRole(Role role)
        {
            if (!_roles.Any(x => x.Id == role.Id))
                throw new IdentityDomainException("Cannot remove role from user. User does not have role");

            _roles.Remove(role);
        }

        public void AddClaim(Claim claim)
        {
            if (_claims.Any(x => x.Type == claim.Type))
                throw new IdentityDomainException("Cannot add claim. User already has claim");

            _claims.Add(claim);
        }

        public void RemoveClaim(Claim claim)
        {
            if (!_claims.Any(x => x.Type == claim.Type))
                throw new IdentityDomainException("Cannot remove claim. User does not have claim");

            _claims.Remove(claim);
        }

        public void Delete()
        {
            AddDomainEvent(new UserDeletedDomainEvent(this));
        }

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            if (username.Length > 100)
                throw new ArgumentException("username cannot exceed 100 characters");
        }
    }
}
