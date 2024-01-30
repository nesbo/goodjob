using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Commands;

namespace Kontravers.GoodJob.API.Requests.Talent;

public class CreatePersonUpworkRssFeedRequest
{
    public required string AbsoluteFeedUrl { get; set; }
    public required bool AutoSendEmailEnabled { get; set; }
    public required bool AutoGenerateProposalsEnabled { get; set; }
    public int? PreferredProfileId { get; set; }
    public required byte MinimumFetchIntervalInMinutes { get; set; }
    public required string Title { get; set; }
    public CreatePersonUpworkRssFeedCommand ToCommand(IClock clock,
        string personIdText)
    {
        if (!int.TryParse(personIdText, out var personId))
        {
            throw new BadRequestException(nameof(personId));
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
        
        return new CreatePersonUpworkRssFeedCommand(
            clock.UtcNow,
            personId,
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