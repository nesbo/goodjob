using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;

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

    public PersonUpworkRssFeed? GetUpworkRssFeed(int feedId)
    {
        return _upworkRssFeeds.SingleOrDefault(x => x.Id == feedId);
    }

    public void UpdateUpworkRssFeed(UpdatePersonUpworkRssFeedCommand command)
    {
        var feed = GetUpworkRssFeed(command.PersonFeedId);
        if (feed is null)
        {
            throw new NotFoundException("Upwork feed not found");
        }
        
        feed.Update(command);
    }

    public void CreateProfile(CreatePersonProfileCommand command, DateTime insertedUtc)
    {
        var existingProfile = _profiles.SingleOrDefault(x => x.Title == command.Title);
        if (existingProfile is not null)
        {
            throw new DuplicateEntityException("Profile with this title already exists");
        }
        
        var profile = new Profile(command.CreatedUtc, insertedUtc, command.Title,
            command.Description, command.Skills);
        _profiles.Add(profile);
    }

    public bool HasProfile(int profileId)
    {
        return _profiles.Any(x => x.Id == profileId);
    }

    public Profile GetProfile(int profileId)
    {
        return _profiles.Single(p => p.Id == profileId);
    }

    public void UpdateProfile(UpdatePersonProfileCommand command)
    {
        var profileId = int.Parse(command.ProfileId);
        if (!HasProfile(profileId))
        {
            throw new NotFoundException("Profile not found");
        }
        
        var profile = GetProfile(profileId);
        profile.Update(command);
    }
}