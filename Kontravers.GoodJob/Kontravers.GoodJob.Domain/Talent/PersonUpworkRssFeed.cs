namespace Kontravers.GoodJob.Domain.Talent;

public class PersonUpworkRssFeed : IEntity
{
    protected PersonUpworkRssFeed() { }
    
    public PersonUpworkRssFeed(int personId, string rootUrl, DateTime lastFetchedAtUtc,
        byte minFetchIntervalInMinutes, string relativeUrl, DateTime createdUtc, DateTime insertedUtc)
    {
        PersonId = personId;
        RootUrl = rootUrl;
        LastFetchedAtUtc = lastFetchedAtUtc;
        MinFetchIntervalInMinutes = minFetchIntervalInMinutes;
        RelativeUrl = relativeUrl;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public int PersonId { get; }
    public string RootUrl { get; }
    public string RelativeUrl { get; }
    public DateTime LastFetchedAtUtc { get; }
    public byte MinFetchIntervalInMinutes { get; }
}