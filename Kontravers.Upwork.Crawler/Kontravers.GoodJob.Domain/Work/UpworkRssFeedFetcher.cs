using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Kontravers.GoodJob.Domain.Work;

public class UpworkRssFeedFetcher
{
    // TODO: catalog persons in Work domain
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly ILogger<UpworkRssFeedFetcher> _logger;
    private readonly IClock _clock;

    public UpworkRssFeedFetcher(IPersonQueryRepository personQueryRepository,
        ILogger<UpworkRssFeedFetcher> logger, IClock clock)
    {
        _personQueryRepository = personQueryRepository;
        _logger = logger;
        _clock = clock;
    }
    
    public async Task StartFetchingAllAsync(CancellationToken cancellationToken)
    { 
        _logger.LogInformation("Starting fetching all Upwork RSS feeds");
        
        _logger.LogTrace("Loading all persons");
        
        // TODO: catalog persons in Work domain
        var persons = await _personQueryRepository.ListAllAsync(cancellationToken);
        if (!persons.Any())
        {
            _logger.LogInformation("No persons found");
            return;
        }
        
        _logger.LogTrace("Starting fetching Upwork RSS feeds for {PersonCount} persons",
            persons.Length);
        
        var personFetchTasks = persons
            .Select(person => FetchPersonUpworkRssFeedsAsync(person, cancellationToken))
            .ToArray();
        
        try
        {
            await Task.WhenAll(personFetchTasks);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching Upwork RSS feeds");
        }
        
        _logger.LogInformation("Finished fetching all Upwork RSS feeds");
    }

    private Task FetchPersonUpworkRssFeedsAsync(Person person, CancellationToken cancellationToken)
    {
        foreach (var personUpworkRssFeed in person.UpworkRssFeeds)
        {
            var feedMinFetchInterval = TimeSpan.FromMinutes(personUpworkRssFeed.MinFetchIntervalInMinutes);
            if (personUpworkRssFeed.LastFetchedAtUtc + feedMinFetchInterval > _clock.UtcNow)
            {
                _logger.LogTrace("Skipping fetching Upwork RSS feed {UpworkRssFeedId} for person {PersonId} " +
                                 "because it was fetched recently at {LastFetchedUtc}",
                    personUpworkRssFeed.Id, person.Id, personUpworkRssFeed.LastFetchedAtUtc);
                continue;
            }
            
            using var restClient = new RestClient(personUpworkRssFeed.RootUrl);
            var request = new RestRequest(personUpworkRssFeed.RelativeUrl);
        }
    }
}