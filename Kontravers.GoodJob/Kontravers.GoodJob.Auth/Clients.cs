using IdentityServer4.Models;

namespace Kontravers.GoodJob.Auth;

public class Clients
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "goodjob-api",
                ClientName = "GoodJob API",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                RequireClientSecret = false,
                RedirectUris = { "https://localhost:5001/signin-oidc", "https://oauth.pstmn.io/v1/callback" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "email" }
            }
        };
    }
}