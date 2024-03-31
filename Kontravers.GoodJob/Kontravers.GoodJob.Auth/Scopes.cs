using IdentityServer4.Models;
using Kontravers.GoodJob.Infra.Shared;

namespace Kontravers.GoodJob.Auth;

public static class Scopes
{

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope(AuthConstants.PersonTalentScope)
            {
                UserClaims = new List<string>
                {
                    AuthConstants.UserIdScope,
                    "role"
                }
            },
            new ApiScope(AuthConstants.PersonWorkScope)
            {
                UserClaims = new List<string>
                {
                    AuthConstants.UserIdScope,
                    "role"
                }
            }
        };
    }
}