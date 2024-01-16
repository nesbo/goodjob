namespace Kontravers.GoodJob.Domain.Talent;

public class PersonUpworkRssFeed : IEntity
{
    public PersonUpworkRssFeed(int personId, string rootUrl, DateTime lastFetchedAtUtc,
        byte minFetchIntervalInMinutes, string relativeUrl)
    {
        PersonId = personId;
        RootUrl = rootUrl;
        LastFetchedAtUtc = lastFetchedAtUtc;
        MinFetchIntervalInMinutes = minFetchIntervalInMinutes;
        RelativeUrl = relativeUrl;
    }

    public int Id { get; protected set; }
    public int PersonId { get; }
    public string RootUrl { get; }
    public string RelativeUrl { get; }
    public DateTime LastFetchedAtUtc { get; }
    public byte MinFetchIntervalInMinutes { get; }
}