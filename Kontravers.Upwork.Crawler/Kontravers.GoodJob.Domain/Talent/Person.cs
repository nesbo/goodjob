namespace Kontravers.GoodJob.Domain.Talent;

public class Person : IAggregate
{
    private List<PersonUpworkRssFeed> _upworkRssFeeds = new ();
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public bool IsEnabled { get; }
    public string OrganisationId { get; }
    
    public IReadOnlyCollection<PersonUpworkRssFeed> UpworkRssFeeds  {
        get => _upworkRssFeeds;
        init => _upworkRssFeeds = value.ToList();
    }

    public Person(bool isEnabled, string email, string name, int id, string organisationId)
    {
        IsEnabled = isEnabled;
        Email = email;
        Name = name;
        Id = id;
        OrganisationId = organisationId;
    }
}