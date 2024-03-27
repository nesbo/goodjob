using IdentityServer4.Models;
using Kontravers.GoodJob.Infra.Shared;

namespace Kontravers.GoodJob.Auth;

public static class Scopes
{

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope(AuthConstants.PersonTalentScope),
            new ApiScope(AuthConstants.PersonWorkScope),
            new ApiScope(AuthConstants.ProfileScope),
            new ApiScope(AuthConstants.OpenIdScope)
        };
    }
}