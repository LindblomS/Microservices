namespace Services.Identity.Contracts.Models
{
    using System;
    using System.Collections.Generic;

    public record ClaimReadModel(string Type, string Value);
    public record RoleReadModel(string Id, string DisplayName, IList<ClaimReadModel> Claims);
    public record RoleWithoutClaimsReadModel(string Id, string DisplayName);
    public record UserReadModel(Guid Id, string Username, IList<ClaimReadModel> Claims, IList<RoleWithoutClaimsReadModel> Roles);
}
