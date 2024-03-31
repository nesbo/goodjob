namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class PersonUpworkRssFeedViewModel(PersonUpworkRssFeed upworkRssFeed)
{
    public string Id { get; set; } = upworkRssFeed.Id.ToString();
    public string AbsoluteFeedUrl { get; set; } = upworkRssFeed.AbsoluteUrl;
    public string Title { get; set; } = upworkRssFeed.Title;
    public int MinimumFetchIntervalInMinutes { get; set; } = upworkRssFeed.MinFetchIntervalInMinutes;
    public DateTime LastFetchTimeUtc { get; set; } = upworkRssFeed.LastFetchedAtUtc;
    public bool AutoSendEmailEnabled { get; set; } = upworkRssFeed.AutoSendEmail;
    public bool AutoGenerateProposalsEnabled { get; set; } = upworkRssFeed.AutoGenerateProposals;
    public int? PreferredProfileId { get; set; } = upworkRssFeed.PreferredProfileId;
    public DateTime CreatedUtc { get; set; } = upworkRssFeed.CreatedUtc;
}