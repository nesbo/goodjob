using IdentityServer4.Models;

namespace Kontravers.GoodJob.Auth;

public class Resources
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
            new IdentityResource("userId", new[] { "userId" })
        };
    }
}