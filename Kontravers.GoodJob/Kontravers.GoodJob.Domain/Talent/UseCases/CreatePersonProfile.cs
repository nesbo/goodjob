using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class CreatePersonProfile : RequestHandlerAsync<CreatePersonProfileCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<CreatePersonProfile> _logger;
    private readonly IClock _clock;

    public CreatePersonProfile(IPersonRepository personRepository,
        ILogger<CreatePersonProfile> logger, IClock clock)
    {
        _personRepository = personRepository;
        _logger = logger;
        _clock = clock;
    }
    
    
    public override async Task<CreatePersonProfileCommand> HandleAsync(CreatePersonProfileCommand command,
        CancellationToken cancellationToken = new ())
    {
        _logger.LogInformation("Creating person profile for person {PersonId}", command.PersonId);
        
        var personId = int.Parse(command.PersonId);
        var person = await _personRepository.GetAsync(personId, cancellationToken);
        
        if (person is null)
        {
            _logger.LogError("Person {PersonId} not found", command.PersonId);
            throw new NotFoundException("Person not found");
        }
        
        person.CreateProfile(command, _clock.UtcNow);
        
        _logger.LogInformation("Person profile created for person {PersonId}. Saving to database",
            command.PersonId);

        await _personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}