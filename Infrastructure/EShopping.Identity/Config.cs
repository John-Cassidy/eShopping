using Duende.IdentityServer.Models;

namespace EShopping.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalogapi"),
            new ApiScope("basketapi"),
            new ApiScope("catalogapi.read"),
            new ApiScope("catalogapi.write"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            // list of microservices
            new ApiResource("Catalog", "Catalog.API")
            {
                 Scopes = {"catalogapi.read", "catalogapi.write"}
            },
            new ApiResource("Basket", "Basket.API")
            {
                Scopes = {"basketapi"}
            },
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
           new Client
            {
                ClientName = "Catalog API Client",
                ClientId = "CatalogApiClient",
                ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"catalogapi.read", "catalogapi.write"}
            },
            new Client {
                ClientName = "Basket API Client",
                ClientId = "BasketApiClient",
                ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"basketapi"}
            }

            // // interactive client using code flow + pkce
            // new Client
            // {
            //     ClientId = "interactive",
            //     ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            //     AllowedGrantTypes = GrantTypes.Code,

            //     RedirectUris = { "https://localhost:44300/signin-oidc" },
            //     FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //     PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            //     AllowOfflineAccess = true,
            //     AllowedScopes = { "openid", "profile", "scope2" }
            // },
        };
}
