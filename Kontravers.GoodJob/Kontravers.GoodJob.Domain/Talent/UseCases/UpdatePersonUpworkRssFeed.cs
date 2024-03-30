using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class UpdatePersonUpworkRssFeed : RequestHandlerAsync<UpdatePersonUpworkRssFeedCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<UpdatePersonUpworkRssFeed> _logger;

    public UpdatePersonUpworkRssFeed(IPersonRepository personRepository,
        ILogger<UpdatePersonUpworkRssFeed> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }

    public override async Task<UpdatePersonUpworkRssFeedCommand> HandleAsync(UpdatePersonUpworkRssFeedCommand command,
        CancellationToken cancellationToken = new())
    {
        _logger.LogInformation("Starting update upwork rss feed for person {PersonId}, feed {UpworkFeedId}",
            command.PersonId, command.PersonFeedId);
        
        var person = await _personRepository.GetAsync(command.PersonId, cancellationToken);
        if (person is null)
        {
            _logger.LogWarning("Person {PersonId} not found", command.PersonId);
            throw new NotFoundException("Person not found");
        }
        
        person.UpdateUpworkRssFeed(command);
        
        _logger.LogInformation("Finished update upwork rss feed for person {PersonId}, feed {UpworkFeedId}",
            command.PersonId, command.PersonFeedId);

        await _personRepository.SaveChangesAsync(cancellationToken);

        return await base.HandleAsync(command, cancellationToken);
    }
}