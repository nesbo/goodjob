using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class CreatePersonUpworkRssFeed: RequestHandlerAsync<CreatePersonUpworkRssFeedCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<CreatePersonUpworkRssFeed> _logger;
    private readonly IClock _clock;

    public CreatePersonUpworkRssFeed(IPersonRepository personRepository,
        ILogger<CreatePersonUpworkRssFeed> logger, IClock clock)
    {
        _personRepository = personRepository;
        _logger = logger;
        _clock = clock;
    }
    
    public override async Task<CreatePersonUpworkRssFeedCommand> HandleAsync(CreatePersonUpworkRssFeedCommand command,
        CancellationToken cancellationToken = new ())
    {
        _logger.LogInformation("Creating person upwork rss feed for person {PersonId}", command.PersonId);
        
        var person = await _personRepository.GetAsync(command.PersonId, cancellationToken);
        
        if (person is null)
        {
            _logger.LogError("Person {PersonId} not found", command.PersonId);
            throw new NotFoundException("Person not found");
        }
        
        person.CreateUpworkRssFeed(command, _clock);
        await _personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}