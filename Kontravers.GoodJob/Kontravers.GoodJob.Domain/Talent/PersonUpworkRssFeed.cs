using Kontravers.GoodJob.Domain.Talent.Commands;

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
    public string RootUrl { get; private set; }
    public string RelativeUrl { get; private set; }

    public string AbsoluteUrl
    {
        get
        {
            var baseUri = new Uri(RootUrl);
            var relativeUri = new Uri(RelativeUrl, UriKind.Relative);
            var url = new Uri(baseUri, relativeUri);
            return url.ToString();
        }
    }

    public DateTime LastFetchedAtUtc { get; }
    public byte MinFetchIntervalInMinutes { get; private set; }
    public int? PreferredProfileId { get; private set; }
    public bool AutoGenerateProposals { get; private set; }
    public bool AutoSendEmail { get; private set; }
    public string Title { get; private set; }

    public void Update(UpdatePersonUpworkRssFeedCommand command)
    {
        MinFetchIntervalInMinutes = command.MinFetchIntervalInMinutes;
        AutoGenerateProposals = command.AutoGenerateProposalsEnabled;
        AutoSendEmail = command.AutoSendEmailEnabled;
        Title = command.Title;
        PreferredProfileId = command.PreferredProfileId;
        RootUrl = command.RootUrl;
        RelativeUrl = command.RelativeUrl;
    }
}