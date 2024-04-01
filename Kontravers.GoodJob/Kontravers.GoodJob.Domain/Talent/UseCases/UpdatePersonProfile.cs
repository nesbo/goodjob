using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class UpdatePersonProfile(
    IPersonRepository personRepository,
    ILogger<UpdatePersonProfile> logger)
    : RequestHandlerAsync<UpdatePersonProfileCommand>
{
    public override async Task<UpdatePersonProfileCommand> HandleAsync(UpdatePersonProfileCommand command,
        CancellationToken cancellationToken = new ())
    {
        logger.LogInformation("Updating person profile for person");
        
        var person = await personRepository.GetByUserIdAsync(command.UserId, cancellationToken);
        
        if (person is null)
        {
            logger.LogError("Person not found");
            throw new NotFoundException("Person not found");
        }
        
        person.UpdateProfile(command);
        
        logger.LogInformation("Person profile updated for person {PersonId}. Saving to database",
            person.Id);
        await personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}