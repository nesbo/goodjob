namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class GetPersonProfileQuery(string userId, string profileId)
{
    public string UserId { get; } = userId;
    public string ProfileId { get; } = profileId;
}