namespace Kontravers.GoodJob.Domain.Talent;

public class PersonUpworkRssFeed : IEntity
{
    protected PersonUpworkRssFeed() { }
    
    public PersonUpworkRssFeed(int personId, string rootUrl, DateTime lastFetchedAtUtc,
        byte minFetchIntervalInMinutes, string relativeUrl, DateTime createdUtc,
        DateTime insertedUtc, bool autoGenerateProposals, bool autoSendEmail,
        string title, int? preferredProfileId = null)
    {
        PersonId = personId;
        RootUrl = rootUrl;
        LastFetchedAtUtc = lastFetchedAtUtc;
        MinFetchIntervalInMinutes = minFetchIntervalInMinutes;
        RelativeUrl = relativeUrl;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
        AutoGenerateProposals = autoGenerateProposals;
        AutoSendEmail = autoSendEmail;
        PreferredProfileId = preferredProfileId;
        Title = title;
    }

    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public int PersonId { get; }
    public string RootUrl { get; }
    public string RelativeUrl { get; }
    public DateTime LastFetchedAtUtc { get; }
    public byte MinFetchIntervalInMinutes { get; }
    public int? PreferredProfileId { get; }
    public bool AutoGenerateProposals { get; }
    public bool AutoSendEmail { get; }
    public string Title { get; }
}