using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPersonUpworkRssFeedQueryHandler
{
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly ILogger<GetPersonUpworkRssFeedQueryHandler> _logger;

    public GetPersonUpworkRssFeedQueryHandler(IPersonQueryRepository personQueryRepository,
        ILogger<GetPersonUpworkRssFeedQueryHandler> logger)
    {
        _personQueryRepository = personQueryRepository;
        _logger = logger;
    }
    
    public async Task<PersonUpworkRssFeedViewModel> GetAsync(GetPersonUpworkRssFeedQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching person upwork rss feed for person {PersonId} and upwork rss feed {UpworkRssFeedId}",
            query.PersonId, query.UpworkRssFeedId);

        var personId = int.Parse(query.PersonId);
        var person = await _personQueryRepository.GetAsync(personId, cancellationToken);

        if (person is null)
        {
            throw new NotFoundException("Person not found");
        }
        
        var feedId = int.Parse(query.UpworkRssFeedId);
        var upworkRssFeed = person.GetUpworkRssFeed(feedId);
        
        if (upworkRssFeed is null)
        {
            throw new NotFoundException("Upwork rss feed not found");
        }
        
        // merge complete url from root and relative url from upworkRssFeed object
        var baseUrl = new Uri(upworkRssFeed.RootUrl);
        var url = new Uri(baseUrl, upworkRssFeed.RelativeUrl);
        
        return new PersonUpworkRssFeedViewModel
        {
            Id = upworkRssFeed.Id.ToString(),
            RssFeedUrl = url.ToString(),
            Title = upworkRssFeed.Title,
            MinimumFetchIntervalInMinutes = upworkRssFeed.MinFetchIntervalInMinutes,
            LastFetchTimeUtc = upworkRssFeed.LastFetchedAtUtc,
            AutoSendEmailEnabled = upworkRssFeed.AutoSendEmail,
            AutoGenerateProposalEnabled = upworkRssFeed.AutoGenerateProposals,
            PreferredProfileId = upworkRssFeed.PreferredProfileId,
            CreatedUtc = upworkRssFeed.CreatedUtc
        };
    }
    
}