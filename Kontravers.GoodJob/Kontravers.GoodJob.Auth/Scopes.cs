using IdentityServer4.Models;

namespace Kontravers.GoodJob.Auth;

public class Scopes
{
    public static string PersonTalent = "person-talent";
    public static string PersonWork = "person-work";
    public static string Profile = "profile";
    public static string OpenId = "openid";
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope(PersonTalent),
            new ApiScope(PersonWork),
            new ApiScope(Profile),
            new ApiScope(OpenId)
        };
    }
}