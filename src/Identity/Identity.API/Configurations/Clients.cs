namespace Identity.API.Configurations
{
    using IdentityServer4.Models;
    using System.Collections.Generic;

    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "postmanclient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedScopes = new[] {"customer.read", "order.read"},
                }
            };
        }
    }
}
