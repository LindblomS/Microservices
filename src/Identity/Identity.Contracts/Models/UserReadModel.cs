namespace Services.Identity.Contracts.Models
{
    using System;
    using System.Collections.Generic;

    public record UserReadModel(Guid Id, string Username, IList<ClaimReadModel> Claims, IList<RoleWithoutClaimsReadModel> Roles);
}
