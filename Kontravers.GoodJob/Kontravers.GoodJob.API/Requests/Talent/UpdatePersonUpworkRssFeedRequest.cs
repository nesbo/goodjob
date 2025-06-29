using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Commands;

namespace Kontravers.GoodJob.API.Requests.Talent;

public class UpdatePersonUpworkRssFeedRequest
{
    public required string AbsoluteFeedUrl { get; set; }
    public required bool AutoSendEmailEnabled { get; set; }
    public required bool AutoGenerateProposalsEnabled { get; set; }
    public int? PreferredProfileId { get; set; }
    public required byte MinimumFetchIntervalInMinutes { get; set; }
    public required string Title { get; set; }
    
    public UpdatePersonUpworkRssFeedCommand ToCommand(IClock clock,
        string userId, string personFeedIdText)
    {
        if (!int.TryParse(personFeedIdText, out var personFeedId))
        {
            throw new BadRequestException(nameof(personFeedId));
        }

        Uri uri;
        try
        {
            uri = new Uri(AbsoluteFeedUrl);
        }
        catch (Exception)
        {
            throw new BadRequestException(nameof(AbsoluteFeedUrl));
        }
        
        return new UpdatePersonUpworkRssFeedCommand(
            clock.UtcNow,
            userId,
            personFeedId,
            uri.Scheme + Uri.SchemeDelimiter + uri.Host,
            uri.PathAndQuery,
            AutoSendEmailEnabled,
            AutoGenerateProposalsEnabled,
            PreferredProfileId,
            MinimumFetchIntervalInMinutes,
            Title
        );
    }
}