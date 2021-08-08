namespace Services.Identity.Contracts.Models
{
    using System.Collections.Generic;

    public record RoleReadModel(string Id, string DisplayName, IList<ClaimReadModel> Claims);
}
