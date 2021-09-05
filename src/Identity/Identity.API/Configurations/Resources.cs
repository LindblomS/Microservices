namespace Identity.API.Configurations
{
    using IdentityServer4.Models;
    using System.Collections.Generic;

    internal class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "order",
                    Scopes = {"order.read", "order.write"},
                    ApiSecrets = { new("secret".Sha256()) }
                },
                new ApiResource
                {
                    Name = "user",
                    Scopes = {"user.write"},
                    ApiSecrets = { new("secret".Sha256())}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new("order.read"),
                new("order.write"),
                new("user.write")
                //new("id")
            };
        }
    }
}
