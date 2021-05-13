namespace Identity.API.Configurations
{
    using IdentityServer4;
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
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"customer.read", "order.read"},
                },
                new Client
                {
                    ClientId = "mvcclient",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:5004/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5004/Home/Index" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
            };
        }
    }
}
