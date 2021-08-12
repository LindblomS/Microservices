namespace Services.User.Domain.AggregateModels.Role
{
    using Services.User.Domain.SeedWork;
    using Services.User.Domain.Events;
    using Services.User.Domain.Exceptions;
    using Services.User.Domain.ValueObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Role : Entity, IAggregateRoot
    {
        private string _displayName;
        private readonly List<Claim> _claims;

        public Role(string id, string displayName)
        {
            ValidateId(id);
            ValidateDisplayName(displayName);

            Id = id;
            _displayName = displayName;
            _claims ??= new List<Claim>();
        }

        public Role(string id, string displayName, IEnumerable<Claim> claims) 
            : this(id, displayName)
        {
            _claims = claims?.ToList() ?? throw new ArgumentNullException(nameof(claims));
        }

        public string Id { get; }
        public string DisplayName => _displayName;
        public IReadOnlyList<Claim> Claims => _claims;

        public void ChangeDisplayName(string displayName)
        {
            ValidateDisplayName(displayName);
            _displayName = displayName;
        }

        public void AddClaim(Claim claim)
        {
            if (_claims.Any(x => x.Type == claim.Type))
                throw new IdentityDomainException("Cannot add claim. Role already has claim");

            _claims.Add(claim);
        }

        public void RemoveClaim(Claim claim)
        {
            if (!_claims.Any(x => x.Type == claim.Type))
                throw new IdentityDomainException("Cannot remove claim. Role does not have claim");

            _claims.Remove(claim);
        }

        public void Delete()
        {
            AddDomainEvent(new RoleDeletedDomainEvent(this));
        }

        private void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            if (id.Length > 25)
                throw new ArgumentException("id cannot exceed 25 characters");
        }

        private void ValidateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            if (displayName.Length > 50)
                throw new ArgumentException("display name cannot exceed 50 characters");
        }
    }
}
