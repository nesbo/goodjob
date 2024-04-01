using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class CreatePersonUpworkRssFeed(
    IPersonRepository personRepository,
    ILogger<CreatePersonUpworkRssFeed> logger,
    IClock clock)
    : RequestHandlerAsync<CreatePersonUpworkRssFeedCommand>
{
    public override async Task<CreatePersonUpworkRssFeedCommand> HandleAsync(
        CreatePersonUpworkRssFeedCommand command, CancellationToken cancellationToken = new ())
    {
        logger.LogInformation("Creating person upwork rss feed");
        
        var person = await personRepository.GetByUserIdAsync(command.UserId, cancellationToken);
        
        if (person is null)
        {
            logger.LogError("Person not found");
            throw new NotFoundException("Person not found");
        }
        
        person.CreateUpworkRssFeed(command, clock);
        await personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}