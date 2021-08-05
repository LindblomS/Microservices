namespace Services.Identity.Contracts.Models
{
    using System.Collections.Generic;

    public record Role(string Id, string DisplayName, IList<Claim> Claims);
}
