using IdentityServer4.Models;
using Kontravers.GoodJob.Infra.Shared;

namespace Kontravers.GoodJob.Auth;

public static class Resources
{
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return Enumerable.Empty<ApiResource>();
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource(AuthConstants.UserIdScope, new[] { "userId" })
        };
    }
}