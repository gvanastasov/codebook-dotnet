using IdentityServer4.Models;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new List<ApiScope>
        {
            new ApiScope("webapp", "Web Application")
        };

    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new Client
            {
                ClientId = "webapp_client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { "http://localhost:5064/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5064/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "webapp" }
            }
        };
}
