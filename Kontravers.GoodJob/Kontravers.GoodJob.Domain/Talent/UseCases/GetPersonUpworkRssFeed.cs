using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPersonUpworkRssFeed
{
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly ILogger<GetPersonUpworkRssFeed> _logger;

    public GetPersonUpworkRssFeed(IPersonQueryRepository personQueryRepository,
        ILogger<GetPersonUpworkRssFeed> logger)
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

        return new PersonUpworkRssFeedViewModel(upworkRssFeed);
    }
    
    public async Task<PersonUpworkRssFeedViewModel> GetAsync(GetPersonUpworkRssFeedByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching person upwork rss feed for user {UserId} and upwork rss feed {UpworkRssFeedId}",
            query.UserId, query.UpworkRssFeedId);

        var person = await _personQueryRepository.GetByUserId(query.UserId, cancellationToken);

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

        return new PersonUpworkRssFeedViewModel(upworkRssFeed);
    }
}