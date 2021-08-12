namespace Services.User.Domain.AggregateModels.User
{
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Domain.Events;
    using Services.User.Domain.Exceptions;
    using Services.User.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Services.User.Domain.ValueObjects;

    public class User : Entity, IAggregateRoot
    {
        private readonly Guid _id;
        private readonly string _username;
        private readonly string _passwordHash;
        private List<Claim> _claims;
        private List<Role> _roles;

        public User(string username, string passwordHash)
        {
            _id = Guid.NewGuid();

            ValidateUsername(username);
            _username = username;

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentNullException(nameof(passwordHash));

            _passwordHash = passwordHash;

            _claims = new List<Claim>();
            _roles = new List<Role>();
        }

        public User(Guid id, string username, string passwordHash,  IEnumerable<Claim> claims, IEnumerable<Role> roles)
        {
            if (id == default(Guid))
                throw new ArgumentNullException(nameof(id));

            _id = id;

            ValidateUsername(username);
            _username = username;

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentNullException(nameof(passwordHash));

            _passwordHash = passwordHash;

            _claims = claims?.ToList() ?? throw new ArgumentNullException(nameof(claims));
            _roles = roles?.ToList() ?? throw new ArgumentNullException(nameof(roles));
        }

        public Guid Id => _id;
        public string Username => _username;
        public string PasswordHash => _passwordHash;
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
