namespace Services.Identity.Contracts.Models
{
    using System;
    using System.Collections.Generic;

    public record User(Guid Id, string Username, IList<Claim> Claims, IList<Role> Roles);
}
