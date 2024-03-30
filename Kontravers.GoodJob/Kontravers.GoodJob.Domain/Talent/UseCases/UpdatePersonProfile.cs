using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class UpdatePersonProfile : RequestHandlerAsync<UpdatePersonProfileCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<UpdatePersonProfile> _logger;

    public UpdatePersonProfile(IPersonRepository personRepository,
        ILogger<UpdatePersonProfile> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public override async Task<UpdatePersonProfileCommand> HandleAsync(UpdatePersonProfileCommand command,
        CancellationToken cancellationToken = new ())
    {
        _logger.LogInformation("Updating person profile for person {PersonId}", command.PersonId);
        
        var personId = int.Parse(command.PersonId);
        var person = await _personRepository.GetAsync(personId, cancellationToken);
        
        if (person is null)
        {
            _logger.LogError("Person {PersonId} not found", command.PersonId);
            throw new NotFoundException("Person not found");
        }
        
        person.UpdateProfile(command);
        
        _logger.LogInformation("Person profile updated for person {PersonId}. Saving to database",
            command.PersonId);
        await _personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}