using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class EnablePerson : RequestHandlerAsync<UserAccountConfirmedEvent>
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<EnablePerson> _logger;

    public EnablePerson(IPersonRepository personRepository,
        ILogger<EnablePerson> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public override async Task<UserAccountConfirmedEvent> HandleAsync(UserAccountConfirmedEvent command,
        CancellationToken cancellationToken = new())
    {
        _logger.LogInformation("Enabling person with user ID {UserId}", command.UserId);
        var person = await _personRepository.GetByUserIdAsync(command.UserId, cancellationToken);
        
        if (person is null)
        {
            _logger.LogWarning("Person with user ID {UserId} not found", command.UserId);
            return await base.HandleAsync(command, cancellationToken);
        }
        
        person.Enable();
        await _personRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Person with user ID {UserId} enabled", command.UserId);
        
        return await base.HandleAsync(command, cancellationToken);
    }
}