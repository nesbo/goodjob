using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPersonProfile(
    IPersonQueryRepository personQueryRepository,
    ILogger<GetPersonProfile> logger)
{
    public async Task<PersonProfileViewModel> GetAsync(GetPersonProfileQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching person profile for person and profile {ProfileId}",
            query.ProfileId);
        
        var person = await personQueryRepository.GetByUserId(query.UserId, cancellationToken);
        
        if (person == null)
        {
            logger.LogError("Person not found");
            throw new NotFoundException($"Person not found");
        }

        var profileId = int.Parse(query.ProfileId);
        if (!person.HasProfile(profileId))
        {
            logger.LogError("Profile with id {ProfileId} not found", query.ProfileId);
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