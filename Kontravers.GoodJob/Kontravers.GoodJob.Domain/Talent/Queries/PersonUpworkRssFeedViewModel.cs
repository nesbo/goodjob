namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class PersonUpworkRssFeedViewModel
{
    public required string Id { get; set; }
    public required string RssFeedUrl { get; set; }
    public required string Title { get; set; }
    public required int MinimumFetchIntervalInMinutes { get; set; }
    public required DateTime LastFetchTimeUtc { get; set; }
    public required bool AutoSendEmailEnabled { get; set; }
    public required bool AutoGenerateProposalEnabled { get; set; }
    public int? PreferredProfileId { get; set; }
    public required DateTime CreatedUtc { get; set; }
}