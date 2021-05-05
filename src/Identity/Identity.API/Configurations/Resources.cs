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
                    Name = "customer",
                    Scopes = new[] {"customer.read", "customer.write"},
                    ApiSecrets = new[] { new Secret("secret".Sha256()) }
                },
                new ApiResource
                {
                    Name = "order",
                    Scopes = new[] {"order.read", "order.write"},
                    ApiSecrets = new[] { new Secret("secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("customer.read"),
                new ApiScope("customer.write"),
                new ApiScope("order.read"),
                new ApiScope("order.write")
            };
        }
    }
}
