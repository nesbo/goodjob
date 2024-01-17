namespace Kontravers.GoodJob.Domain.Talent;

public class Person : IAggregate
{
    private List<PersonUpworkRssFeed> _upworkRssFeeds = new ();
    public int Id { get; }
    public DateTime CreatedUtc { get; }
    public DateTime InsertedUtc { get; }
    public string Name { get; }
    public string Email { get; }
    public bool IsEnabled { get; }
    public string OrganisationId { get; }
    
    public IReadOnlyCollection<PersonUpworkRssFeed> UpworkRssFeeds  {
        get => _upworkRssFeeds;
        init => _upworkRssFeeds = value.ToList();
    }

    protected Person() { }
    
    public Person(bool isEnabled, string email, string name, int id, string organisationId,
        DateTime createdUtc, DateTime insertedUtc)
    {
        IsEnabled = isEnabled;
        Email = email;
        Name = name;
        Id = id;
        OrganisationId = organisationId;
        CreatedUtc = createdUtc;
        InsertedUtc = insertedUtc;
    }
}