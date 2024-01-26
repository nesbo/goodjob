namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class GetPersonUpworkRssFeedQuery
{
    public GetPersonUpworkRssFeedQuery(string personId, string upworkRssFeedId)
    {
        PersonId = personId;
        UpworkRssFeedId = upworkRssFeedId;
    }
    public string PersonId { get; set; }
    public string UpworkRssFeedId { get; set; }
}