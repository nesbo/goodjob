using System.Xml;
using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Messaging.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.Services;

public class UpworkRssFeedFetcher
{
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly ILogger<UpworkRssFeedFetcher> _logger;
    private readonly IClock _clock;
    private readonly ICommandPublisher _commandPublisher;
    private readonly IHttpClient _httpClient;

    public UpworkRssFeedFetcher(IPersonQueryRepository personQueryRepository,
        ILogger<UpworkRssFeedFetcher> logger, IClock clock,
        ICommandPublisher commandPublisher, IHttpClient httpClient)
    {
        _personQueryRepository = personQueryRepository;
        _logger = logger;
        _clock = clock;
        _commandPublisher = commandPublisher;
        _httpClient = httpClient;
    }
    
    public async Task StartFetchingAllAsync(CancellationToken cancellationToken)
    { 
        _logger.LogInformation("Starting fetching all Upwork RSS feeds");
        
        _logger.LogTrace("Loading all persons");
        
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
            _logger.LogError(e, "Error fetching Upwork RSS feeds for some persons");
            throw;
        }
        
        _logger.LogInformation("Finished fetching all Upwork RSS feeds");
    }

    private Task FetchPersonUpworkRssFeedsAsync(Person person, CancellationToken cancellationToken)
    {
        var personUpworkRssFeedFetchTasks = person
            .UpworkRssFeeds
            .Select(personUpworkRssFeed => Task.Factory.StartNew(async () =>
            {
                var feedMinFetchInterval = TimeSpan.FromMinutes(personUpworkRssFeed.MinFetchIntervalInMinutes);
                if (personUpworkRssFeed.LastFetchedAtUtc + feedMinFetchInterval > _clock.UtcNow)
                {
                    _logger.LogTrace("Skipping fetching Upwork RSS feed {UpworkRssFeedId} for person {PersonId} "
                                     + "because it was fetched recently at {LastFetchedUtc}",
                        personUpworkRssFeed.Id, person.Id, personUpworkRssFeed.LastFetchedAtUtc);
                    return;
                }

                string response;
                try
                {
                    response = await _httpClient.GetStringAsync(personUpworkRssFeed.RootUrl,
                        personUpworkRssFeed.RelativeUrl, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        "Error fetching Upwork RSS feed {UpworkRssFeedId} for person {PersonId}",
                        personUpworkRssFeed.Id, person.Id);
                    throw;
                }

                if (string.IsNullOrEmpty(response))
                {
                    _logger.LogError("Error fetching Upwork RSS feed {UpworkRssFeedId} for person {PersonId}. "
                                     + "Response was empty",
                        personUpworkRssFeed.Id, person.Id);
                    return;
                }
                
                await ParseRssFeedAndPublishJobCommandsAsync(personUpworkRssFeed, response, cancellationToken);

            }, cancellationToken))
            .ToList();

        return Task.WhenAll(personUpworkRssFeedFetchTasks);
    }

    private async Task ParseRssFeedAndPublishJobCommandsAsync(PersonUpworkRssFeed personUpworkRssFeed,
        string responseContent, CancellationToken cancellationToken)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(responseContent);
        var items = xmlDocument.SelectNodes("/rss/channel/item");
        
        if (items == null || items.Count == 0)
        {
            _logger.LogTrace("No items found in Upwork RSS feed {UpworkRssFeedId} for person {PersonId}",
                personUpworkRssFeed.Id, personUpworkRssFeed.PersonId);
            return;
        }
        
        _logger.LogTrace("Found {ItemCount} items in Upwork RSS feed {UpworkRssFeedId} for person {PersonId}",
            items.Count, personUpworkRssFeed.Id, personUpworkRssFeed.PersonId);

        for (var i = 0; i < items.Count; i++)
        {
            var item = items.Item(i);
            var command = CreateJobCommand.FromUpworkRssFeedItem(item!, _clock.UtcNow);
            await _commandPublisher.PublishAsync(command, cancellationToken);
        }
    }
}