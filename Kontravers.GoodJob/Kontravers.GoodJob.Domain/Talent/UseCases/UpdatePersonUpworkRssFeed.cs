using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class UpdatePersonUpworkRssFeed(
    IPersonRepository personRepository,
    ILogger<UpdatePersonUpworkRssFeed> logger)
    : RequestHandlerAsync<UpdatePersonUpworkRssFeedCommand>
{
    public override async Task<UpdatePersonUpworkRssFeedCommand> HandleAsync(UpdatePersonUpworkRssFeedCommand command,
        CancellationToken cancellationToken = new())
    {
        logger.LogInformation("Starting update upwork rss feed for person {PersonId}, feed {UpworkFeedId}",
            command.UserId, command.PersonFeedId);
        
        var person = await personRepository.GetByUserIdAsync(command.UserId, cancellationToken);
        if (person is null)
        {
            logger.LogWarning("Person not found");
            throw new NotFoundException("Person not found");
        }
        
        person.UpdateUpworkRssFeed(command);
        
        logger.LogInformation("Finished update upwork rss feed for person, feed {UpworkFeedId}",
            command.PersonFeedId);

        await personRepository.SaveChangesAsync(cancellationToken);

        return await base.HandleAsync(command, cancellationToken);
    }
}