using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class CreatePerson : RequestHandlerAsync<UserCreatedEvent>
{
    private readonly IClock _clock;
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<CreatePerson> _logger;

    public CreatePerson(IClock clock, IPersonRepository personRepository, ILogger<CreatePerson> logger)
    {
        _clock = clock;
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public override async Task<UserCreatedEvent> HandleAsync(UserCreatedEvent command,
        CancellationToken cancellationToken = new())
    {
        _logger.LogInformation("Creating person for user {UserId}", command.UserId);
        
        if (!int.TryParse(command.OrganizationId, out var organizationId))
        {
            _logger.LogError("Invalid organization id {OrganizationId}", command.OrganizationId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        var personExistsForOrganisation = await _personRepository
            .ExistsForOrganisationAsync(command.Email, organizationId, cancellationToken);
        
        if (personExistsForOrganisation)
        {
            _logger.LogWarning("Person already exists for organization {OrganizationId}", organizationId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        var person = new Person(false, command.Email, command.Name, organizationId,
            command.OccurredOn, _clock.UtcNow, command.UserId);
        
        await _personRepository.AddAsync(person, cancellationToken);
        await _personRepository.SaveChangesAsync(cancellationToken);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}