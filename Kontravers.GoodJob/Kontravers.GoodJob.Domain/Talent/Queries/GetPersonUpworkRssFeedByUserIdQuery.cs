namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class GetPersonUpworkRssFeedByUserIdQuery(string userId, string upworkRssFeedId)
{
    public string UserId { get; set; } = userId;
    public string UpworkRssFeedId { get; set; } = upworkRssFeedId;
}