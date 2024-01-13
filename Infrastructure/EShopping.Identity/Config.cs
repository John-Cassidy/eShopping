using Duende.IdentityServer;
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
            new ApiScope("catalogapi", "Catalog API"),
            new ApiScope("basketapi", "Basket API"),
            new ApiScope("eshoppinggateway", "EShopping Gateway"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            // list of microservices
            new ApiResource("Catalog", "Catalog.API")
            {
                 Scopes = {"catalogapi"}
            },
            new ApiResource("Basket", "Basket.API")
            {
                Scopes = {"basketapi"}
            },
            new ApiResource("EShoppingGateway", "EShopping Gateway")
            {
                Scopes = {"eshoppinggateway", "catalogapi", "basketapi" }
            },
            new ApiResource("eshoppingAngular", "EShopping Angular")
            {
                Scopes = {"eshoppinggateway", "catalogapi", "basketapi"}
            }
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
                AllowedScopes = {"catalogapi"}
            },
            new Client {
                ClientName = "Basket API Client",
                ClientId = "BasketApiClient",
                ClientSecrets = {new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"basketapi"}
            },
            new Client
            {
                ClientName = "EShopping Gateway Client",
                ClientId = "EShoppingGatewayClient",
                ClientSecrets = {new Secret("5c7fd5c5-61a7-4668-ac57-2b4591ec26d2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "eshoppinggateway", "basketapi", "catalogapi"}
            },
            new Client
            {
                ClientName = "Angular Client",
                ClientId = "angular-client",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = new List<string>
                {
                    "https://localhost:4201/signin-callback",
                    "https://localhost:4201/assets/silent-callback.html",
                    "https://localhost:9099/signin-oidc",
                },
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                Enabled = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "eshoppinggateway",
                },
                AllowedCorsOrigins = new List<string>
                {
                    "https://localhost:4201"
                },
                RequireClientSecret = false,
                AllowRememberConsent = false,
                PostLogoutRedirectUris = new List<string>
                {
                    "https://localhost:4201/signout-callback",
                    "https://localhost:9099/signout-callback-oidc"
                },
                RequireConsent = false,
                AccessTokenLifetime = 3600,
                ClientSecrets = new List<Secret>
                {
                    new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())
                }
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
