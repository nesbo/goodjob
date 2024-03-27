using IdentityServer4.Models;
using Kontravers.GoodJob.Infra.Shared;

namespace Kontravers.GoodJob.Auth;

public class Clients
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
                RedirectUris = { "https://localhost:5001/signin-oidc", "https://oauth.pstmn.io/v1/callback" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "email", 
                    AuthConstants.PersonTalentScope, AuthConstants.PersonWorkScope }
            }
        };
    }
}