using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPersonProfile
{
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly ILogger<GetPersonProfile> _logger;

    public GetPersonProfile(IPersonQueryRepository personQueryRepository,
        ILogger<GetPersonProfile> logger)
    {
        _personQueryRepository = personQueryRepository;
        _logger = logger;
    }
    
    public async Task<PersonProfileViewModel> GetAsync(GetPersonProfileQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching person profile for person {PersonId} and profile {ProfileId}",
            query.PersonId, query.ProfileId);
        
        var personId = int.Parse(query.PersonId);
        var person = await _personQueryRepository.GetAsync(personId, cancellationToken);
        
        if (person == null)
        {
            _logger.LogError("Person with id {PersonId} not found", query.PersonId);
            throw new NotFoundException($"Person with id {personId} not found");
        }

        var profileId = int.Parse(query.ProfileId);
        if (!person.HasProfile(profileId))
        {
            _logger.LogError("Profile with id {ProfileId} not found", query.ProfileId);
            throw new NotFoundException($"Profile with id {profileId} not found");
        }
        
        var profile = person.GetProfile(profileId);

        return new PersonProfileViewModel
        {
            Id = profile.Id.ToString(),
            Title = profile.Title,
            Description = profile.Description,
            Skills = profile.Skills,
            CreatedUtc = profile.CreatedUtc
        };
    }
}