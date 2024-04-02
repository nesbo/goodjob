using IdentityServer4.Models;
using Kontravers.GoodJob.Infra.Shared;

namespace Kontravers.GoodJob.Auth;

public static class Clients
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new()
            {
                ClientId = "goodjob-api-client",
                ClientName = "GoodJob API Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                RequireClientSecret = false,
                RedirectUris =
                {
                    "https://localhost:5001/signin-oidc",
                    "https://oauth.pstmn.io/v1/callback",
                    "https://goodjob.kontrave.rs/callback",
                    "http://localhost:8010/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                AllowedScopes =
                {
                    AuthConstants.PersonTalentScope,
                    AuthConstants.PersonWorkScope,
                    AuthConstants.ProfileScope,
                    AuthConstants.OpenIdScope,
                    AuthConstants.UserIdScope
                }
            }
        };
    }
}