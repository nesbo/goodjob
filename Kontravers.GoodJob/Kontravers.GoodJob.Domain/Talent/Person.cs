namespace Kontravers.GoodJob.Domain.Talent;

public class Person : IAggregate
{
    private List<PersonUpworkRssFeed> _upworkRssFeeds = new ();
    private List<Profile> _profiles = new ();
    public int Id { get; protected set; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public string Name { get; }
    public string Email { get; }
    public bool IsEnabled { get; }
    public int OrganisationId { get; }
    
    public IReadOnlyCollection<PersonUpworkRssFeed> UpworkRssFeeds
    {
        get => _upworkRssFeeds;
        init => _upworkRssFeeds = value.ToList();
    }
    
    public IReadOnlyCollection<Profile> Profiles
    {
        get => _profiles;
        init => _profiles = value.ToList();
    }

    protected Person() { }
    
    public Person(bool isEnabled, string email, string name, int organisationId,
        DateTime createdUtc, DateTime insertedUtc)
    {
        IsEnabled = isEnabled;
        Email = email;
        Name = name;
        OrganisationId = organisationId;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
    }

    public void AddUpworkRssFeed(string rootUrl, string relativeUrl, DateTime lastFetchedAtUtc,
        byte minFetchIntervalInMinutes, DateTime createdUtc, DateTime insertedUtc,
        bool autoGenerateProposals, bool autoSendEmails, string title,
        int? preferredPortfolioId = null)
    {
        var upworkRssFeed = new PersonUpworkRssFeed(Id, rootUrl, lastFetchedAtUtc,
            minFetchIntervalInMinutes, relativeUrl, createdUtc, insertedUtc,
            autoGenerateProposals, autoSendEmails, title, preferredPortfolioId);
        _upworkRssFeeds.Add(upworkRssFeed);
    }
}